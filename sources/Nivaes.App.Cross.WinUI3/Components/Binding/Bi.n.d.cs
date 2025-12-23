namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;
    using MvvmCross;
    using MvvmCross.Platforms.WinUi.Binding;
    using Nivaes.App.Cross;

    // ReSharper disable InconsistentNaming
    public static class Bi
    // ReSharper restore InconsistentNaming
    {
        static Bi()
        {
            MvxDesignTimeChecker.Check();
        }

        // ReSharper disable InconsistentNaming
        public static readonly DependencyProperty ndProperty =
            // ReSharper restore InconsistentNaming
            DependencyProperty.RegisterAttached("nd",
                                                typeof(string),
                                                typeof(Bi),
                                                new PropertyMetadata(null, CallBackWhenndIsChanged));

        public static string Getnd(DependencyObject obj)
        {
            return obj.GetValue(ndProperty) as string;
        }

        public static void Setnd(
            DependencyObject obj,
            string value)
        {
            obj.SetValue(ndProperty, value);
        }

        private static IMvxBindingCreator _bindingCreator;

        private static IMvxBindingCreator BindingCreator
        {
            get
            {
                _bindingCreator = _bindingCreator ?? ResolveBindingCreator();
                return _bindingCreator;
            }
        }

        private static IMvxBindingCreator ResolveBindingCreator()
        {
            IMvxBindingCreator toReturn;
            if (!Mvx.IoCProvider.TryResolve<IMvxBindingCreator>(out toReturn))
            {
                throw new CrossException("Unable to resolve the binding creator - have you initialized Windows Binding");
            }

            return toReturn;
        }

        private static void CallBackWhenndIsChanged(
            object sender,
            DependencyPropertyChangedEventArgs args)
        {
            // bindingCreator may be null in the designer currently
            var bindingCreator = BindingCreator;

            bindingCreator?.CreateBindings(sender, args, ParseBindingDescriptions);
        }

        private static IEnumerable<CrossBindingDescription> ParseBindingDescriptions(string bindingText)
        {
            if (CrossSingleton<ICrossBindingSingletonCache>.Instance == null)
                return Array.Empty<CrossBindingDescription>();

            return CrossSingleton<ICrossBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingText);
        }
    }
}
