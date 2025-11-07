namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static partial class CrossBindingContextOwnerExtensions
    {
        public static void CreateBindingContext(this ICrossBindingContextOwner view)
        {
            throw new NotImplementedException();
            //view.BindingContext = Cross.IoCProvider.Resolve<ICrossBindingContext>();
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public static void CreateBindingContext(this ICrossBindingContextOwner view, string bindingText)
        {
            throw new NotImplementedException();
            //view.BindingContext = Cross.IoCProvider.Resolve<ICrossBindingContext>().Init(null, view, bindingText);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public static void CreateBindingContext(this ICrossBindingContextOwner view,
                                                IEnumerable<CrossBindingDescription> bindings)
        {
            throw new NotImplementedException();
            //view.BindingContext = Cross.IoCProvider.Resolve<ICrossBindingContext>().Init(null, view, bindings);
        }

        /*
		 * This overload removed at present - it caused confusion on iOS
		 * because it could lead to the bindings being described before
		 * the table cells were fully available
        public static void DelayBind(this ICrossBindingContextOwner view, params ICrossApplicable[] applicables)
        {
            view.BindingContext.DelayBind(() => applicables.Apply());
        }
        */

        public static void DelayBind(this ICrossBindingContextOwner view, Action bindingAction)
        {
            view.BindingContext.DelayBind(bindingAction);
        }

        public static void AddBinding(this ICrossBindingContextOwner view, object target, ICrossUpdateableBinding binding, object clearKey = null)
        {
            if (clearKey == null)
            {
                view.BindingContext.RegisterBinding(target, binding);
            }
            else
            {
                view.BindingContext.RegisterBindingWithClearKey(clearKey, target, binding);
            }
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void AddBinding(this ICrossBindingContextOwner view, object target,
                                      CrossBindingDescription bindingDescription, object clearKey = null)
        {
            var descriptions = new[] { bindingDescription };
            view.AddBindings(target, descriptions, clearKey);
        }

        public static void AddBindings(this ICrossBindingContextOwner view, object target, IEnumerable<ICrossUpdateableBinding> bindings, object clearKey = null)
        {
            if (bindings == null)
                return;

            foreach (var binding in bindings)
                view.AddBinding(target, binding, clearKey);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public static void AddBindings(this ICrossBindingContextOwner view, object target, string bindingText, object clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingText);
            view.AddBindings(target, bindings, clearKey);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public static void AddBindings(this ICrossBindingContextOwner view, object target,
                                       IEnumerable<CrossBindingDescription> bindingDescriptions, object clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingDescriptions);
            view.AddBindings(target, bindings, clearKey);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void AddBindings(this ICrossBindingContextOwner view,
                                       IDictionary<object, string> bindingMap,
                                       object clearKey = null)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value, clearKey);
            }
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void AddBindings(this ICrossBindingContextOwner view,
                                       IDictionary<object, IEnumerable<CrossBindingDescription>> bindingMap,
                                       object clearKey = null)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value, clearKey);
            }
        }
    }
}
