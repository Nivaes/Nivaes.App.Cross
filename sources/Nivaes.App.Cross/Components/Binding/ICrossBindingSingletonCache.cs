namespace Nivaes.App.Cross
{
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.ExpressionParse;

    public interface ICrossBindingSingletonCache
    {
        ICrossAutoValueConverters AutoValueConverters { get; }
        ICrossBindingDescriptionParser BindingDescriptionParser { get; }
        ICrossLanguageBindingParser LanguageParser { get; }
        ICrossPropertyExpressionParser PropertyExpressionParser { get; }
        ICrossValueConverterLookup ValueConverterLookup { get; }
        ICrossBindingNameLookup DefaultBindingNameLookup { get; }
        ICrossBinder Binder { get; }
        ICrossSourceBindingFactory SourceBindingFactory { get; }
        ICrossTargetBindingFactory TargetBindingFactory { get; }
        ICrossSourceStepFactory SourceStepFactory { get; }
        ICrossValueCombinerLookup ValueCombinerLookup { get; }
        ICrossMainThreadAsyncDispatcher MainThreadDispatcher { get; }
    }
}
