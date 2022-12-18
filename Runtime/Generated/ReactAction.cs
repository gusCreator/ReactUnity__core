
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;

namespace ReactUnity.Helpers
{
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action listener = null;
            listener = () =>
            {
                try
                {
                    cb.Call();
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke()
        {
            _delegate?.Invoke();
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction operator +(ReactAction action, Action listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction operator -(ReactAction action, Action listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1> listener = null;
            listener = (arg1) =>
            {
                try
                {
                    cb.Call(arg1);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1)
        {
            _delegate?.Invoke(arg1);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1> operator +(ReactAction<T1> action, Action<T1> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1> operator -(ReactAction<T1> action, Action<T1> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2> listener = null;
            listener = (arg1, arg2) =>
            {
                try
                {
                    cb.Call(arg1, arg2);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2)
        {
            _delegate?.Invoke(arg1, arg2);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2> operator +(ReactAction<T1, T2> action, Action<T1, T2> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2> operator -(ReactAction<T1, T2> action, Action<T1, T2> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2, T3>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2, T3> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2, T3> listener = null;
            listener = (arg1, arg2, arg3) =>
            {
                try
                {
                    cb.Call(arg1, arg2, arg3);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2, T3> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            _delegate?.Invoke(arg1, arg2, arg3);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2, T3> operator +(ReactAction<T1, T2, T3> action, Action<T1, T2, T3> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2, T3> operator -(ReactAction<T1, T2, T3> action, Action<T1, T2, T3> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2, T3, T4>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2, T3, T4> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2, T3, T4> listener = null;
            listener = (arg1, arg2, arg3, arg4) =>
            {
                try
                {
                    cb.Call(arg1, arg2, arg3, arg4);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2, T3, T4> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            _delegate?.Invoke(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4> operator +(ReactAction<T1, T2, T3, T4> action, Action<T1, T2, T3, T4> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4> operator -(ReactAction<T1, T2, T3, T4> action, Action<T1, T2, T3, T4> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2, T3, T4, T5>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2, T3, T4, T5> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2, T3, T4, T5> listener = null;
            listener = (arg1, arg2, arg3, arg4, arg5) =>
            {
                try
                {
                    cb.Call(arg1, arg2, arg3, arg4, arg5);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2, T3, T4, T5> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            _delegate?.Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5> operator +(ReactAction<T1, T2, T3, T4, T5> action, Action<T1, T2, T3, T4, T5> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5> operator -(ReactAction<T1, T2, T3, T4, T5> action, Action<T1, T2, T3, T4, T5> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2, T3, T4, T5, T6> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2, T3, T4, T5, T6> listener = null;
            listener = (arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                try
                {
                    cb.Call(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2, T3, T4, T5, T6> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            _delegate?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6> operator +(ReactAction<T1, T2, T3, T4, T5, T6> action, Action<T1, T2, T3, T4, T5, T6> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6> operator -(ReactAction<T1, T2, T3, T4, T5, T6> action, Action<T1, T2, T3, T4, T5, T6> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2, T3, T4, T5, T6, T7> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2, T3, T4, T5, T6, T7> listener = null;
            listener = (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                try
                {
                    cb.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2, T3, T4, T5, T6, T7> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            _delegate?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6, T7> operator +(ReactAction<T1, T2, T3, T4, T5, T6, T7> action, Action<T1, T2, T3, T4, T5, T6, T7> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6, T7> operator -(ReactAction<T1, T2, T3, T4, T5, T6, T7> action, Action<T1, T2, T3, T4, T5, T6, T7> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2, T3, T4, T5, T6, T7, T8> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2, T3, T4, T5, T6, T7, T8> listener = null;
            listener = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                try
                {
                    cb.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2, T3, T4, T5, T6, T7, T8> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            _delegate?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6, T7, T8> operator +(ReactAction<T1, T2, T3, T4, T5, T6, T7, T8> action, Action<T1, T2, T3, T4, T5, T6, T7, T8> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6, T7, T8> operator -(ReactAction<T1, T2, T3, T4, T5, T6, T7, T8> action, Action<T1, T2, T3, T4, T5, T6, T7, T8> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
    /// <summary>
    ///   A wrapper for the <see cref="ReactUnity.Helpers.Callback" /> class to provide convienient access to the
    ///   delegates and events in ReactUnity from both JavaScript and TypeScript as well as C# as normal events.
    /// </summary>
    /// <example>
    ///   <code language="csharp">
    ///     var callback = new ReactAction&lt;string&gt;();
    ///     var listener = (str) => Console.WriteLine(str);
    ///     callback += listener;
    ///     callback.Invoke("It works! From C#");
    ///     callback -= listener;
    ///   </code>
    /// </example>
    /// <example>
    ///   <code language="js">
    ///     useEffect(() => {
    ///       let unsubscribe = Global.Instance.Delegate.AddListener((str) => console.log(str));
    ///       Global.Instance.Delegate.Invoke("It works! From TypeScript");
    ///       return () => unsubscribe();
    ///     }, []);
    ///   </code>
    /// </example>
    class ReactAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        /// <summary>
        ///   The underlying action.
        /// </summary>
        private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> _delegate { get; set; }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener. This method should be only called from React.
        /// </summary>
        [Obsolete("This method should be only called from React. Use the typed AddListener method or += and -= operators.")]
        public Action AddListener(object callback)
        {
            var cb = Callback.From(callback);
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listener = null;
            listener = (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                try
                {
                    cb.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                }
                catch
                {
                    _delegate -= listener;
                }
            };
            _delegate += listener;
            return () => _delegate -= listener;
        }

        /// <summary>
        ///   Adds the listener to the action, and returns a function that removes the listener.
        /// </summary>
        public Action AddListener(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listener)
        {
            _delegate += listener;
            return () => _delegate -= listener;
        }

        ///  <summary>
        ///    Invokes the action.
        ///  </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            _delegate?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        ///   Creates a new ReactAction from a callback.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> operator +(ReactAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listener)
        {
            action._delegate += listener;
            return action;
        }
        /// <summary>
        ///   Removes the listener from the action.
        /// </summary>
        public static ReactAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> operator -(ReactAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> listener)
        {
            action._delegate -= listener;
            return action;
        }
  }
}

