using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

public static partial class CrossBindingContextOwnerExtensions
{
    extension(ICrossBindingContextOwner view)
    {
        public void CreateBindingContext()
        {
            view.BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingContext>();
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public void CreateBindingContext(string bindingText)
        {
            view.BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingContext>().Init(null, view, bindingText);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public void CreateBindingContext(IEnumerable<CrossBindingDescription> bindings)
        {
            view.BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingContext>().Init(null, view, bindings);
        }
        public void DelayBind(params ICrossApplicable[] applicables)
        {
            view.BindingContext?.DelayBind(() => applicables.Apply());
        }

        public void DelayBind(Action bindingAction)
        {
            view.BindingContext?.DelayBind(bindingAction);
        }

        public void AddBinding(object target, ICrossUpdateableBinding binding, object? clearKey = null)
        {
            if (clearKey == null)
            {
                view.BindingContext?.RegisterBinding(target, binding);
            }
            else
            {
                view.BindingContext?.RegisterBindingWithClearKey(clearKey, target, binding);
            }
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public void AddBinding(object? target,
                                      CrossBindingDescription bindingDescription, object? clearKey = null)
        {
            var descriptions = new[] { bindingDescription };
            view.AddBindings(target, descriptions, clearKey);
        }

        public void AddBindings(object target, IEnumerable<ICrossUpdateableBinding> bindings, object? clearKey = null)
        {
            if (bindings == null)
                return;

            foreach (var binding in bindings)
                view.AddBinding(target, binding, clearKey);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public void AddBindings(object target, string bindingText, object? clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext?.DataContext, target, bindingText);
            view.AddBindings(target, bindings, clearKey);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public void AddBindings(object target,
                                       IEnumerable<CrossBindingDescription> bindingDescriptions, object clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext?.DataContext, target, bindingDescriptions);
            view.AddBindings(target, bindings, clearKey);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public void AddBindings(IDictionary<object, string> bindingMap, object? clearKey = null)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value, clearKey);
            }
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public void AddBindings(IDictionary<object, IEnumerable<CrossBindingDescription>> bindingMap, object? clearKey = null)
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
