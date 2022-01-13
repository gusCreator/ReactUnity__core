using System;
using System.Collections.Generic;
using ReactUnity.Helpers;
using ReactUnity.Styling;

namespace ReactUnity.Scripting.DomProxies
{
    public class DocumentProxy
    {
        public HeadProxy head;
        public string Origin;
        public ReactContext Context;

        public DocumentProxy(ReactContext context, string origin)
        {
            head = new HeadProxy();
            Origin = origin;
            Context = context;
        }

        public object createElement(string type)
        {
            if (type == "script") return new ScriptProxy(this);
            else if (type == "style") return new StyleProxy(this);
            else if (type == "link") return new LinkProxy(this);
            else return Context.CreateComponent(type, "");
        }

        public string createTextNode(string text)
        {
            return text;
        }

        public object querySelector(string query)
        {
            if (query == "head") return head;
            return Context.Host.QuerySelector(query);
        }

        public object querySelectorAll(string query)
        {
            if (query == "head") return new List<object> { head };
            return Context.Host.QuerySelectorAll(query);
        }

        public object getElementById(string id)
        {
            // TODO: handle efficiently
            return Context.Host.QuerySelector("#" + id);
        }

        public List<IDomElementProxy> getElementsByTagName(string tagName)
        {
            return head.Children.FindAll(x => x.tagName == tagName);
        }
    }

    public interface IDomElementProxy
    {
        string tagName { get; }

        void OnAppend();
        void OnRemove();

        void setAttribute(object key, object value);
        void removeAttribute(object key);

        void appendChild(string text);
        void removeChild(string text);
    }

    public abstract class DomElementProxyBase
    {
        public abstract string tagName { get; }
        public int nodeType => 1;
        public object nextSibling => null;
        public string id = "";

        Dictionary<string, object> attributes = new Dictionary<string, object>();

        public void setAttribute(object key, object value) => attributes[key?.ToString() ?? ""] = value;
        public void removeAttribute(object key) => attributes.Remove(key?.ToString() ?? "");
        public bool hasAttribute(object key) => attributes.ContainsKey(key?.ToString() ?? "");
        public object getAttribute(object key) => attributes.TryGetValue(key?.ToString() ?? "", out var val) ? val : default;
    }

    public class HeadProxy : DomElementProxyBase
    {
        public override string tagName { get; } = "head";
        public List<IDomElementProxy> Children { get; } = new List<IDomElementProxy>();

        public void appendChild(IDomElementProxy child)
        {
            child.OnAppend();
            Children.Add(child);
        }

        public void removeChild(IDomElementProxy child)
        {
            child.OnRemove();
            Children.Remove(child);
        }

        public void insertBefore(IDomElementProxy child, object before)
        {
            child.OnAppend();
            var ind = before is IDomElementProxy ip ? Children.IndexOf(ip) : -1;
            if (ind >= 0) Children.Insert(ind, child);
            else Children.Add(child);
        }
    }

    public class ScriptProxy : DomElementProxyBase, IDomElementProxy
    {
        public override string tagName { get; } = "script";
        public string src { get; set; }
        public string charset { get; set; }
        public string crossOrigin { get; set; }
        public float timeout { get; set; }

        private Callback onloadCallback { get; set; }
        private Callback onerrorCallback { get; set; }
        private IReactComponent component;

        public object onload
        {
            set { onloadCallback = new Callback(value); }
            get => new Action(() => onloadCallback.Call());
        }

        public object onerror
        {
            set { onerrorCallback = new Callback(value); }
            get => new Action(() => onerrorCallback.Call());
        }

        public DocumentProxy document;
        public HeadProxy parentNode;

        public ScriptProxy(DocumentProxy document)
        {
            this.document = document;
            parentNode = document.head;
        }

        public void OnAppend()
        {
            var script = document.Context.CreateComponent("script", "");
            script.AddEventListener("onLoad", onloadCallback);
            script.AddEventListener("onError", onerrorCallback);
            script.SetParent(document.Context.Host);
            script.SetProperty("source", src);
        }

        public void OnRemove()
        {
            component?.Remove();
            component = null;
        }

        public void appendChild(string text)
        {
            throw new NotImplementedException();
        }

        public void removeChild(string text)
        {
            throw new NotImplementedException();
        }
    }

    public class StyleProxy : DomElementProxyBase, IDomElementProxy
    {
        public class StyleSheetProxy
        {
            StyleProxy Proxy;

            public string cssText
            {
                set
                {
                    Proxy.childNodes.Clear();
                    Proxy.childNodes.Add(value);
                    Proxy.ProcessNodes();
                }
            }

            public StyleSheetProxy(StyleProxy pr)
            {
                Proxy = pr;
            }
        }

        public override string tagName { get; } = "style";
        public List<string> childNodes = new List<string>();
        public string firstChild => childNodes.Count > 0 ? childNodes[0] : default;

        public StyleSheet Sheet = null;
        public StyleSheetProxy styleSheet;

        public bool enabled;

        public DocumentProxy document;
        public HeadProxy parentNode;

        public StyleProxy(DocumentProxy document)
        {
            this.document = document;
            parentNode = document.head;
            styleSheet = new StyleSheetProxy(this);
        }

        public void OnAppend()
        {
            enabled = true;
            ProcessNodes();
        }

        public void OnRemove()
        {
            if (Sheet != null) document.Context.RemoveStyle(Sheet);
            Sheet = null;
        }

        public void appendChild(string text)
        {
            childNodes.Add(text);

            if (enabled) ProcessNodes();
        }

        public void removeChild(string text)
        {
            childNodes.Remove(text);

            if (enabled) ProcessNodes();
        }

        void ProcessNodes()
        {
            if (Sheet != null) document.Context.RemoveStyle(Sheet);
            Sheet = document.Context.InsertStyle(string.Join("\n", childNodes));
        }
    }

    public class LinkProxy : DomElementProxyBase, IDomElementProxy
    {
        public override string tagName { get; } = "link";
        public DocumentProxy document;
        public HeadProxy parentNode;
        private IReactComponent component;

        public string rel = "";

        public string type;

        public string href;


        private Callback onloadCallback { get; set; }
        private Callback onerrorCallback { get; set; }

        public object onload
        {
            set { onloadCallback = new Callback(value); }
            get => new Action(() => onloadCallback.Call());
        }

        public object onerror
        {
            set { onerrorCallback = new Callback(value); }
            get => new Action(() => onerrorCallback.Call());
        }


        public LinkProxy(DocumentProxy document)
        {
            this.document = document;
            parentNode = document.head;
        }

        public void OnAppend()
        {
            var tag = "";

            if (type == "text/css") tag = "style";
            else if (type == "text/javascript") tag = "script";

            if (!string.IsNullOrWhiteSpace(tag))
            {
                var cmp = component = document.Context.CreateComponent(tag, "") as SourceProxyComponent;
                cmp.AddEventListener("onLoad", onloadCallback);
                cmp.AddEventListener("onError", onerrorCallback);
                if (type == "text/css") cmp.SetProperty("scope", ":root");
                cmp.SetParent(document.Context.Host);
                cmp.SetProperty("source", href);
            }
        }

        public void OnRemove()
        {
            component?.Remove();
            component = null;
        }

        public void appendChild(string text) { }

        public void removeChild(string text) { }
    }
}
