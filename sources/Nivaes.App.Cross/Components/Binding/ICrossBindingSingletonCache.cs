namespace Nivaes.App.Cross
{
    public interface ICrossBindingSingletonCache
    {
        [Obsolete("", true)]
        ICrossAutoValueConverters AutoValueConverters { get; }

        ICrossBindingDescriptionParser BindingDescriptionParser { get; }
        ICrossLanguageBindingParser LanguageParser { get; }
        ICrossPropertyExpressionParser PropertyExpressionParser { get; }

        [Obsolete("", true)]
        ICrossValueConverterLookup ValueConverterLookup { get; }

        ICrossBindingNameLookup DefaultBindingNameLookup { get; }
        ICrossBinder Binder { get; }
        ICrossSourceBindingFactory SourceBindingFactory { get; }

        [Obsolete("", true)]
        ICrossTargetBindingFactory TargetBindingFactory { get; }

        ICrossSourceStepFactory SourceStepFactory { get; }

        [Obsolete("", true)]
        ICrossValueCombinerLookup ValueCombinerLookup { get; }

        ICrossMainThreadAsyncDispatcher MainThreadDispatcher { get; }
    }
}
