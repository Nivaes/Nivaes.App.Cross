namespace MvvmCross.Binding
{
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Binding.Combiners;
    using MvvmCross.Binding.ExpressionParse;
    using MvvmCross.Binding.Parse.Binding.Lang;
    using Nivaes.App.Cross;

    public interface IMvxBindingSingletonCache
    {
        IMvxAutoValueConverters AutoValueConverters { get; }
        IMvxBindingDescriptionParser BindingDescriptionParser { get; }
        IMvxLanguageBindingParser LanguageParser { get; }
        IMvxPropertyExpressionParser PropertyExpressionParser { get; }
        IMvxValueConverterLookup ValueConverterLookup { get; }
        IMvxBindingNameLookup DefaultBindingNameLookup { get; }
        IMvxBinder Binder { get; }
        ICrossSourceBindingFactory SourceBindingFactory { get; }
        ICrossTargetBindingFactory TargetBindingFactory { get; }
        IMvxSourceStepFactory SourceStepFactory { get; }
        IMvxValueCombinerLookup ValueCombinerLookup { get; }
        ICrossMainThreadAsyncDispatcher MainThreadDispatcher { get; }
    }
}
