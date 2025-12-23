namespace Nivaes.App.Cross
{
    public static partial class CrossBindingContextOwnerExtensions
    {
        public static ICrossLanguageBindingParser LanguageParser => CrossBindingSingletonCache.Instance.LanguageParser;

        public static ICrossPropertyExpressionParser PropertyExpressionParser => CrossBindingSingletonCache.Instance.PropertyExpressionParser;

        public static ICrossValueConverterLookup ValueConverterLookup => CrossBindingSingletonCache.Instance.ValueConverterLookup;

        public static ICrossBindingNameLookup DefaultBindingNameLookup => CrossBindingSingletonCache.Instance.DefaultBindingNameLookup;

        public static ICrossBinder Binder => CrossBindingSingletonCache.Instance.Binder;
    }
}
