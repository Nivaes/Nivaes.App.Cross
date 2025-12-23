namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Nivaes.App.Cross;

    public class CrossFromTextBinder
        : ICrossBinder
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public IEnumerable<ICrossUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = CrossBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public IEnumerable<ICrossUpdateableBinding> Bind(object source, object target,
                                                       IEnumerable<CrossBindingDescription> bindingDescriptions)
        {
            if (bindingDescriptions == null)
                return Array.Empty<ICrossUpdateableBinding>();

            return
                bindingDescriptions.Select(description => BindSingle(new CrossBindingRequest(source, target, description)));
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public IEnumerable<ICrossUpdateableBinding> LanguageBind(object source, object target, string bindingText)
        {
            var bindingDescriptions =
                CrossBindingSingletonCache.Instance.BindingDescriptionParser.LanguageParse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public ICrossUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
                                                string partialBindingDescription)
        {
            var bindingDescription =
                CrossBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(partialBindingDescription);
            if (bindingDescription == null)
                return null;

            bindingDescription.TargetName = targetPropertyName;
            var request = new CrossBindingRequest(source, target, bindingDescription);
            return BindSingle(request);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public ICrossUpdateableBinding BindSingle(CrossBindingRequest bindingRequest)
        {
            return new CrossFullBinding(bindingRequest);
        }
    }
}
