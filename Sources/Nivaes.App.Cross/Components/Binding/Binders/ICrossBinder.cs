namespace Nivaes.App.Cross
{
    public interface ICrossBinder
    {
        IEnumerable<ICrossUpdateableBinding> Bind(object? source, object target, string? bindingText);

        IEnumerable<ICrossUpdateableBinding> Bind(object? source, object target,
                                                IEnumerable<CrossBindingDescription> bindingDescriptions);

        IEnumerable<ICrossUpdateableBinding> LanguageBind(object? source, object target, string? bindingText);

        ICrossUpdateableBinding? BindSingle(object? source, object target, string targetPropertyName,
                                         string partialBindingDescription);

        ICrossUpdateableBinding BindSingle(CrossBindingRequest bindingRequest);
    }
}
