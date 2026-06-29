namespace Nivaes.App.Cross
{
    public static partial class CrossBindingContextOwnerExtensions
    {
        public static ICrossLanguageBindingParser LanguageParser => Singleton<CrossBindingSingletonCache>.Instance.LanguageParser;

        public static ICrossPropertyExpressionParser PropertyExpressionParser => Singleton<CrossBindingSingletonCache>.Instance.PropertyExpressionParser;

        [Obsolete("1", true)]
        public static ICrossBindingNameLookup DefaultBindingNameLookup => Singleton<CrossBindingSingletonCache>.Instance.DefaultBindingNameLookup;

        public static ICrossBinder Binder => Singleton<CrossBindingSingletonCache>.Instance.Binder;
    }
}
