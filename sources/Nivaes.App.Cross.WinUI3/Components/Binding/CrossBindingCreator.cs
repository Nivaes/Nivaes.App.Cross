namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.Extensions.Logging;
    using Microsoft.UI.Xaml;

    public abstract class CrossBindingCreator : ICrossBindingCreator
    {
        public void CreateBindings(
            object sender, DependencyPropertyChangedEventArgs args,
            Func<string, IEnumerable<CrossBindingDescription>> parseBindingDescriptions)
        {
            var attachedObject = sender as FrameworkElement;
            if (attachedObject == null)
            {
                CrossLogHost.Default?.LogWarning("Null attached FrameworkElement seen in Bi.nd binding");
                return;
            }

            var text = args.NewValue as string;
            if (string.IsNullOrEmpty(text))
                return;

            var bindingDescriptions = parseBindingDescriptions(text);
            if (bindingDescriptions == null)
                return;

            ApplyBindings(attachedObject, bindingDescriptions);
        }

        protected abstract void ApplyBindings(
            FrameworkElement attachedObject, IEnumerable<CrossBindingDescription> bindingDescriptions);
    }
}
