namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;
    using Nivaes.App.Cross;

    public class MvxMvvmCrossBindingCreator : MvxBindingCreator
    {
        protected override void ApplyBindings(FrameworkElement attachedObject,
                                              IEnumerable<CrossBindingDescription> bindingDescriptions)
        {
            var binder = CrossBindingSingletonCache.Instance.Binder;
            var bindingDescriptionList = bindingDescriptions.ToList();
            var bindings = binder.Bind(attachedObject.DataContext, attachedObject, bindingDescriptionList);
            RegisterBindingsForUpdates(attachedObject, bindings);
        }

        private void RegisterBindingsForUpdates(FrameworkElement attachedObject,
                                                IEnumerable<ICrossUpdateableBinding> bindings)
        {
            if (bindings == null)
                return;

            var bindingsList = GetOrCreateBindingsList(attachedObject);
            foreach (var binding in bindings)
            {
                bindingsList.Add(binding);
            }
        }

        private IList<ICrossUpdateableBinding> GetOrCreateBindingsList(FrameworkElement attachedObject)
        {
            var existing = attachedObject.GetValue(BindingsListProperty) as IList<ICrossUpdateableBinding>;
            if (existing != null)
                return existing;

            // attach the list
            var newList = new List<ICrossUpdateableBinding>();
            attachedObject.SetValue(BindingsListProperty, newList);

            // create a binding watcher for the list
            var binding = new Microsoft.UI.Xaml.Data.Binding();

            bool attached = false;
            Action attachAction = () =>
            {
                if (attached)
                    return;
                BindingOperations.SetBinding(attachedObject, DataContextWatcherProperty, binding);
                attached = true;
            };

            Action detachAction = () =>
            {
                if (!attached)
                    return;

                attachedObject.ClearValue(DataContextWatcherProperty);
                attached = false;
            };
            attachAction();
            attachedObject.Loaded += (o, args) =>
            {
                attachAction();
            };
            attachedObject.Unloaded += (o, args) =>
            {
                detachAction();
            };

            return newList;
        }

        public static readonly DependencyProperty DataContextWatcherProperty = DependencyProperty.Register(
            "DataContextWatcher",
            typeof(object),
            typeof(FrameworkElement),
            new PropertyMetadata(null, DataContext_Changed));

        public static object GetDataContextWatcher(DependencyObject d)
        {
            return d.GetValue(DataContextWatcherProperty);
        }

        public static void SetDataContextWatcher(DependencyObject d, string value)
        {
            d.SetValue(DataContextWatcherProperty, value);
        }

        public static readonly DependencyProperty BindingsListProperty = DependencyProperty.Register(
            "BindingsList",
            typeof(IList<ICrossUpdateableBinding>),
            typeof(FrameworkElement),
            new PropertyMetadata(null));

        public static IList<ICrossUpdateableBinding> GetBindingsList(DependencyObject d)
        {
            return d.GetValue(BindingsListProperty) as IList<ICrossUpdateableBinding>;
        }

        public static void SetBindingsList(DependencyObject d, string value)
        {
            d.SetValue(BindingsListProperty, value);
        }

        private static void DataContext_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            var bindings = frameworkElement?.GetValue(BindingsListProperty) as IList<ICrossUpdateableBinding>;
            if (bindings == null)
                return;

            foreach (var binding in bindings)
            {
                binding.DataContext = e.NewValue;
            }
        }
    }
}
