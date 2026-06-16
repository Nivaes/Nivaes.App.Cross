namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public interface ICrossBinder
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IEnumerable<ICrossUpdateableBinding> Bind(object? source, object target, string bindingText);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IEnumerable<ICrossUpdateableBinding> Bind(object? source, object target,
                                                IEnumerable<CrossBindingDescription> bindingDescriptions);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        IEnumerable<ICrossUpdateableBinding> LanguageBind(object? source, object target, string bindingText);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        ICrossUpdateableBinding? BindSingle(object? source, object target, string targetPropertyName,
                                         string partialBindingDescription);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        ICrossUpdateableBinding BindSingle(CrossBindingRequest bindingRequest);
    }
}
