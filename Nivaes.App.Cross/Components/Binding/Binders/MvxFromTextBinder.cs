// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Binders
{
    using System.Collections.Generic;
    using System.Linq;
    using MvvmCross.Binding.Bindings;

    public class MvxFromTextBinder
        : IMvxBinder
    {
        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = MvxBindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<IMvxUpdateableBinding> LanguageBind(object source, object target, string bindingText)
        {
            var bindingDescriptions =
                MvxBindingSingletonCache.Instance.BindingDescriptionParser.LanguageParse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<IMvxUpdateableBinding> Bind(object source, object target,
                                                       IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            if (bindingDescriptions == null)
                return System.Array.Empty<IMvxUpdateableBinding>();

            return
                bindingDescriptions.Select(description => BindSingle(new MvxBindingRequest(source, target, description)));
        }

        public IMvxUpdateableBinding BindSingle(object source, object target, string targetPropertyName,
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

        public IMvxUpdateableBinding BindSingle(MvxBindingRequest bindingRequest)
        {
            return new MvxFullBinding(bindingRequest);
        }
    }
}
