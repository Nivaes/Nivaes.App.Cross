namespace Nivaes.App.Cross
{
    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.ExpressionParse;
    using MvvmCross.Binding.Parse.Binding.Lang;

    public static partial class MvxBindingContextOwnerExtensions
    {
        public static IMvxLanguageBindingParser LanguageParser => MvxBindingSingletonCache.Instance.LanguageParser;

        public static IMvxPropertyExpressionParser PropertyExpressionParser => MvxBindingSingletonCache.Instance.PropertyExpressionParser;

        public static IMvxValueConverterLookup ValueConverterLookup => MvxBindingSingletonCache.Instance.ValueConverterLookup;

        public static IMvxBindingNameLookup DefaultBindingNameLookup => MvxBindingSingletonCache.Instance.DefaultBindingNameLookup;

        public static IMvxBinder Binder => MvxBindingSingletonCache.Instance.Binder;
    }
}
