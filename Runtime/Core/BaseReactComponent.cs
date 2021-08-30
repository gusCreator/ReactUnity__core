using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Yoga;
using ReactUnity.Helpers;
using ReactUnity.Helpers.TypescriptUtils;
using ReactUnity.StyleEngine;
using ReactUnity.Styling;
using ReactUnity.Visitors;
using UnityEngine;

namespace ReactUnity
{
    public abstract class BaseReactComponent<ContextType> : IReactComponent, IContainerComponent where ContextType : ReactContext
    {
        public ContextType Context { get; }
        ReactContext IReactComponent.Context => Context;
        public IContainerComponent Parent { get; private set; }

        public WatchableObjectRecord Data { get; private set; } = new WatchableObjectRecord();
        public YogaNode Layout { get; private set; }
        public NodeStyle ComputedStyle => StyleState.Active;
        public StyleState StyleState { get; private set; }
        public StateStyles StateStyles { get; private set; }

        [TypescriptRemap("../properties/style", "InlineStyleRemap")]
        public InlineStyles Style { get; protected set; } = new InlineStyles();

        public bool Destroyed { get; protected set; }
        public bool IsPseudoElement { get; set; } = false;
        public string Tag { get; private set; } = "";
        public string TextContent => new TextContentVisitor().Get(this);
        protected virtual string DefaultName => $"<{Tag}>";

        public string ClassName
        {
            get => ClassList.Name;
            set => ClassList.Name = value;
        }
        public ClassList ClassList { get; protected set; }

        private string id;
        public string Id
        {
            get => id;
            set
            {
                id = value?.ToString();
                MarkStyleUpdateWithSiblings(true);
            }
        }
        public abstract string Name { get; set; }

        #region Container Properties
        public bool IsContainer { get; }
        public List<IReactComponent> Children { get; private set; }
        public List<RuleTreeNode<StyleData>> BeforeRules { get; protected set; }
        public List<RuleTreeNode<StyleData>> AfterRules { get; protected set; }
        public IReactComponent BeforePseudo { get; protected set; }
        public IReactComponent AfterPseudo { get; protected set; }


        #endregion


        private bool markedStyleResolve;
        private bool markedForStyleApply;
        private bool markedForLayoutApply;
        private bool markedStyleResolveRecursive;
        private bool isEntering;
        private float enteredAt;
        protected Dictionary<string, List<Callback>> BaseEventHandlers = new Dictionary<string, List<Callback>>();
        protected Dictionary<string, Action> EventHandlerRemovers = new Dictionary<string, Action>();

        protected BaseReactComponent(ContextType context, string tag = "", bool isContainer = true)
        {
            IsContainer = isContainer;
            Children = IsContainer ? new List<IReactComponent>() : null;
            Tag = tag;
            Context = context;
            Style.changed += StyleChanged;
            Data.changed += DataChanged;
            ClassList = new ClassList(this);

            if (context.CalculatesLayout) Layout = new YogaNode();

            StateStyles = new StateStyles(this);
            StateStyles.StartState("enter");
            isEntering = true;
            enteredAt = Context.Timer.AnimationTime;

            StyleState = new StyleState(context);
            StyleState.OnUpdate += OnStylesUpdated;
            StyleState.OnEvent += FireEvent;
        }

        public virtual void Update()
        {
            if (Destroyed) return;
            ApplyEnterLeave();
            if (markedStyleResolve) ResolveStyle(markedStyleResolveRecursive);
            StyleState.Update();
            if (markedForStyleApply) ApplyStyles();
            if (markedForLayoutApply) ApplyLayoutStyles();
        }

        protected void DataChanged(string key, object value, WatchableDictionary<string, object> style)
        {
            MarkStyleUpdateWithSiblings(true);
        }

        protected void StyleChanged(IStyleProperty key, object value, WatchableDictionary<IStyleProperty, object> style)
        {
            MarkForStyleResolving(key == null || key.inherited);
        }

        public void MarkForStyleResolving(bool recursive)
        {
            markedStyleResolveRecursive = markedStyleResolveRecursive || recursive;
            markedStyleResolve = true;
        }

        protected void MarkForStyleApply(bool hasLayout)
        {
            markedForStyleApply = true;
            markedForLayoutApply = markedForLayoutApply || hasLayout;
        }

        public virtual void DestroySelf()
        {
            Destroyed = true;
        }

        public void Destroy()
        {
            SetParent(null);
            DestroySelf();

            if (IsContainer)
            {
                RemoveAfter();
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].Destroy();
                }
                RemoveBefore();
                Children.Clear();
            }
        }

        #region Setters

        public virtual void SetParent(IContainerComponent newParent, IReactComponent relativeTo = null, bool insertAfter = false)
        {
            if (Parent != null) Parent.UnregisterChild(this);

            Parent = newParent;

            if (Parent == null) return;

            relativeTo = relativeTo ?? (insertAfter ? null : newParent.AfterPseudo);

            if (relativeTo == null)
            {
                newParent.RegisterChild(this);
            }
            else
            {
                var ind = newParent.Children.IndexOf(relativeTo);
                if (insertAfter) ind++;

                newParent.RegisterChild(this, ind);
            }

            StyleState.SetParent(newParent.StyleState);
            MarkStyleUpdateWithSiblings(true);
        }


        public void SetEventListener(string eventName, Callback fun)
        {
            if (EventHandlerRemovers.TryGetValue(eventName, out var remover))
            {
                remover?.Invoke();
                EventHandlerRemovers[eventName] = null;
            }

            if (fun != null)
            {
                var newRemover = AddEventListener(eventName, fun);
                EventHandlerRemovers[eventName] = newRemover;
            }
        }

        public virtual Action AddEventListener(string eventName, Callback fun)
        {
            List<Callback> list;
            if (!BaseEventHandlers.TryGetValue(eventName, out list))
                BaseEventHandlers[eventName] = list = new List<Callback>();
            list.Add(fun);

            return () => list.Remove(fun);
        }

        public virtual void FireEvent(string eventName, object arg)
        {
            if (BaseEventHandlers.TryGetValue(eventName, out var existingHandlers))
            {
                foreach (var handler in existingHandlers)
                    handler?.Call(arg, this);
            }
        }

        public virtual void SetData(string propertyName, object value)
        {
            Data[propertyName] = value;
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "id":
                    Id = value?.ToString();
                    return;
                case "name":
                    Name = value is string s ? s : null;
                    return;
                case "className":
                    ClassName = value?.ToString();
                    return;
                default:
#if UNITY_EDITOR
                    Debug.LogWarning($"Unknown property name specified, '{propertyName}'");
#endif
                    return;
            }
        }

        #endregion

        #region Style / Layout

        public virtual void ResolveStyle(bool recursive = false)
        {
            if (Destroyed) return;
            markedStyleResolve = false;
            markedStyleResolveRecursive = false;

            List<RuleTreeNode<StyleData>> matchingRules;
            if (Tag == "_before") matchingRules = Parent.BeforeRules;
            else if (Tag == "_after") matchingRules = Parent.AfterRules;
            else matchingRules = Context.Style.StyleTree.GetMatchingRules(this).ToList();

            var importantIndex = Math.Max(0, matchingRules.FindIndex(x => x.Specifity <= RuleHelpers.ImportantSpecifity));
            var cssStyles = new List<IDictionary<IStyleProperty, object>> { };

            for (int i = 0; i < importantIndex; i++) cssStyles.AddRange(matchingRules[i].Data?.Rules);
            cssStyles.Add(Style);
            for (int i = importantIndex; i < matchingRules.Count; i++) cssStyles.AddRange(matchingRules[i].Data?.Rules);

            var resolvedStyle = new NodeStyle(Context, null, cssStyles);

            StyleState.SetCurrent(resolvedStyle);
            ApplyStyles();
            ApplyLayoutStyles();
            resolvedStyle.MarkChangesSeen();

            if (IsContainer)
            {
                var inheritedChanges = ComputedStyle.HasInheritedChanges;

                if (inheritedChanges || recursive)
                {
                    BeforeRules = Context.Style.StyleTree.GetMatchingBefore(this).ToList();
                    if (BeforeRules.Count > 0) AddBefore();
                    else RemoveBefore();
                    BeforePseudo?.ResolveStyle();

                    foreach (var child in Children)
                        child.ResolveStyle(true);

                    AfterRules = Context.Style.StyleTree.GetMatchingAfter(this).ToList();
                    if (AfterRules.Count > 0) AddAfter();
                    else RemoveAfter();
                    AfterPseudo?.ResolveStyle();
                }
            }
        }

        public void MarkStyleUpdateWithSiblings(bool recursive)
        {
            if (Parent == null) return;

            if (Parent.Children == null)
            {
                MarkForStyleResolving(recursive);
                return;
            }

            var resolve = false;
            foreach (var child in Parent.Children)
            {
                resolve = resolve || child == this;
                if (resolve) child.MarkForStyleResolving(recursive);
            }
        }

        protected abstract void ApplyStylesSelf();
        protected abstract void ApplyLayoutStylesSelf();

        public void ApplyStyles()
        {
            markedForStyleApply = false;
            ApplyEnterLeave();
            ApplyStylesSelf();
        }

        private void ApplyEnterLeave()
        {
            if (isEntering)
            {
                if (StateStyles.Subscribed.Contains("enter"))
                {
                    var enterDuration = ComputedStyle.stateDuration / 1000f;

                    if (Context.Timer.AnimationTime >= enteredAt + enterDuration)
                    {
                        StateStyles.EndState("enter");
                        isEntering = false;
                        MarkStyleUpdateWithSiblings(true);
                    }
                }
                else
                {
                    StateStyles.EndState("enter");
                    isEntering = false;
                }
            }
        }

        public void ApplyLayoutStyles()
        {
            markedForLayoutApply = false;
            ApplyLayoutStylesSelf();
        }

        private void OnStylesUpdated(NodeStyle obj, bool hasLayout)
        {
            MarkForStyleApply(hasLayout);
        }

        #endregion


        #region Component Tree Functions

        public bool Matches(string query)
        {
            var tree = new RuleTree<string>(Context.Parser);
            tree.AddSelector(query);
            return tree.AnyMatches(this, Context.Host);
        }

        public IReactComponent Closest(string query)
        {
            var tree = new RuleTree<string>(Context.Parser);
            tree.AddSelector(query);
            return tree.Closest(this, Context.Host);
        }

        public IReactComponent QuerySelector(string query)
        {
            var tree = new RuleTree<string>(Context.Parser);
            tree.AddSelector(query);
            return tree.GetMatchingChild(this);
        }

        public List<IReactComponent> QuerySelectorAll(string query)
        {
            var tree = new RuleTree<string>(Context.Parser);
            tree.AddSelector(query);
            return tree.GetMatchingChildren(this);
        }

        public void Accept(ReactComponentVisitor visitor)
        {
            visitor.Visit(this);

            if (IsContainer)
            {
                BeforePseudo?.Accept(visitor);
                foreach (var child in Children)
                    child.Accept(visitor);
                AfterPseudo?.Accept(visitor);
            }
        }

        #endregion

        #region Pseudo Element Functions

        public void AddBefore()
        {
            if (!IsContainer || BeforePseudo != null) return;
            var tc = Context.CreatePseudoComponent("_before");
            BeforePseudo = tc;
            tc.SetParent(this, Children.FirstOrDefault());
        }

        public void RemoveBefore()
        {
            BeforePseudo?.Destroy();
            BeforePseudo = null;
        }

        public void AddAfter()
        {
            if (!IsContainer || AfterPseudo != null) return;
            var tc = Context.CreatePseudoComponent("_after");
            AfterPseudo = tc;
            tc.SetParent(this, Children.LastOrDefault(), true);
        }

        public void RemoveAfter()
        {
            AfterPseudo?.Destroy();
            AfterPseudo = null;
        }

        #endregion

        #region Container Functions

        public void RegisterChild(IReactComponent child, int index = -1)
        {
            var accepted = IsContainer && InsertChild(child, index);
            if (accepted)
            {
                if (index >= 0)
                {
                    Children.Insert(index, child);
                    Layout?.Insert(index, child.Layout);
                }
                else
                {
                    Children.Add(child);
                    Layout?.AddChild(child.Layout);
                }
            }
        }

        public void UnregisterChild(IReactComponent child)
        {
            var accepted = IsContainer && DeleteChild(child);
            if (accepted)
            {
                Children.Remove(child);
                Layout?.RemoveChild(child.Layout);
            }
        }

        protected abstract bool InsertChild(IReactComponent child, int index);
        protected abstract bool DeleteChild(IReactComponent child);

        #endregion

        #region Add/Get Component Utilities

        public abstract object GetComponent(Type type);
        public abstract object AddComponent(Type type);

        public CType GetComponent<CType>() where CType : Component
        {
            return GetComponent(typeof(CType)) as CType;
        }

        public CType AddComponent<CType>() where CType : Component
        {
            return AddComponent(typeof(CType)) as CType;
        }

        public CType GetOrAddComponent<CType>() where CType : Component
        {
            var existing = GetComponent<CType>();
            if (existing) return existing;
            return AddComponent<CType>();
        }

        #endregion
    }
}
