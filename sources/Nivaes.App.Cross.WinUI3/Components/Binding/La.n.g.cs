namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;

    // ReSharper disable InconsistentNaming
    public static class La
    // ReSharper restore InconsistentNaming
    {
        static La()
        {
            CrossDesignTimeChecker.Check();
        }

        // ReSharper disable InconsistentNaming
        public static readonly DependencyProperty ngProperty =
            // ReSharper restore InconsistentNaming
            DependencyProperty.RegisterAttached("ng",
                                                typeof(string),
                                                typeof(La),
                                                new PropertyMetadata(null, CallBackWhenngIsChanged));

        public static string Getng(DependencyObject obj)
        {
            return obj.GetValue(ngProperty) as string;
        }

        public static void Setng(
            DependencyObject obj,
            string value)
        {
            obj.SetValue(ngProperty, value);
        }

        private static ICrossBindingCreator _bindingCreator;

        private static ICrossBindingCreator BindingCreator
        {
            get
            {
                throw new NotImplementedException();
                //_bindingCreator = _bindingCreator ?? Cross.IoCProvider.Resolve<ICrossBindingCreator>();
                //return _bindingCreator;
            }
        }

        private static void CallBackWhenngIsChanged(
            object sender,
            DependencyPropertyChangedEventArgs args)
        {
            // bindingCreator may be null in the designer currently
            var bindingCreator = BindingCreator;
            if (bindingCreator == null)
                return;

            bindingCreator.CreateBindings(sender, args, ParseBindingDescriptions);
        }

        private static IEnumerable<CrossBindingDescription> ParseBindingDescriptions(string languageText)
        {
            if (CrossSingleton<ICrossBindingSingletonCache>.Instance == null)
                return null;

            return CrossSingleton<ICrossBindingSingletonCache>.Instance.BindingDescriptionParser.LanguageParse(languageText);
        }
    }
}
