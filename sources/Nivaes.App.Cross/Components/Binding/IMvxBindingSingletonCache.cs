namespace Nivaes.App.Cross
{
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.ExpressionParse;

    public interface IMvxBindingSingletonCache
    {
        ICrossAutoValueConverters AutoValueConverters { get; }
        ICrossBindingDescriptionParser BindingDescriptionParser { get; }
        IMvxLanguageBindingParser LanguageParser { get; }
        IMvxPropertyExpressionParser PropertyExpressionParser { get; }
        ICrossValueConverterLookup ValueConverterLookup { get; }
        ICrossBindingNameLookup DefaultBindingNameLookup { get; }
        ICrossBinder Binder { get; }
        ICrossSourceBindingFactory SourceBindingFactory { get; }
        ICrossTargetBindingFactory TargetBindingFactory { get; }
        IMvxSourceStepFactory SourceStepFactory { get; }
        IMvxValueCombinerLookup ValueCombinerLookup { get; }
        ICrossMainThreadAsyncDispatcher MainThreadDispatcher { get; }
    }
}
