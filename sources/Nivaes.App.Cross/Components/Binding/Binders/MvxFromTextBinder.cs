namespace MvvmCross.Binding.Binders
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using MvvmCross.Binding.Bindings;
    using Nivaes.App.Cross;

    public class MvxFromTextBinder
        : IMvxBinder
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public IEnumerable<ICrossUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public IEnumerable<ICrossUpdateableBinding> Bind(object source, object target,
                                                       IEnumerable<CrossBindingDescription> bindingDescriptions)
        {
            if (bindingDescriptions == null)
                return Array.Empty<ICrossUpdateableBinding>();

            return
                bindingDescriptions.Select(description => BindSingle(new MvxBindingRequest(source, target, description)));
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public IEnumerable<ICrossUpdateableBinding> LanguageBind(object source, object target, string bindingText)
        {
            var bindingDescriptions =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.LanguageParse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public ICrossUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
                                                string partialBindingDescription)
        {
            var bindingDescription =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(partialBindingDescription);
            if (bindingDescription == null)
                return null;

            bindingDescription.TargetName = targetPropertyName;
            var request = new MvxBindingRequest(source, target, bindingDescription);
            return BindSingle(request);
        }

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        public ICrossUpdateableBinding BindSingle(MvxBindingRequest bindingRequest)
        {
            return new CrossFullBinding(bindingRequest);
        }
    }
}
