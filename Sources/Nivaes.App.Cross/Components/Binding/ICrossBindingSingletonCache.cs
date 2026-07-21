namespace Nivaes.App.Cross
{
    // ToDo: Eliminar esta clase.
    public interface ICrossBindingSingletonCache
    {
        ICrossBindingDescriptionParser BindingDescriptionParser { get; }
        ICrossLanguageBindingParser LanguageParser { get; }
        ICrossPropertyExpressionParser PropertyExpressionParser { get; }

        ICrossBindingNameLookup DefaultBindingNameLookup { get; }
        ICrossBinder Binder { get; }
        ICrossSourceBindingFactory SourceBindingFactory { get; }

        ICrossSourceStepFactory SourceStepFactory { get; }

        ICrossMainThreadDispatcher MainThreadDispatcher { get; }
    }
}
