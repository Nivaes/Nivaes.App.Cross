namespace Nivaes.App.Cross
{
    public class CrossFromTextBinder
        : ICrossBinder
    {
        public IEnumerable<ICrossUpdateableBinding> Bind(object? source, object target, string? bindingText)
        {
            var bindingDescriptions = Singleton<CrossBindingSingletonCache>.Instance!.BindingDescriptionParser.Parse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<ICrossUpdateableBinding> Bind(object? source, object target,
                                                       IEnumerable<CrossBindingDescription> bindingDescriptions)
        {
            if (bindingDescriptions == null)
                return Array.Empty<ICrossUpdateableBinding>();

            return bindingDescriptions.Select(description => BindSingle(new CrossBindingRequest(source, target, description)));
        }

        public IEnumerable<ICrossUpdateableBinding> LanguageBind(object? source, object target, string? bindingText)
        {
            var bindingDescriptions =
                Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.LanguageParse(bindingText!);
            return Bind(source, target, bindingDescriptions);
        }

        public ICrossUpdateableBinding? BindSingle(object? source, object target, string targetPropertyName,
                                                string partialBindingDescription)
        {
            var bindingDescription =
                Singleton<CrossBindingSingletonCache>.Instance.BindingDescriptionParser.ParseSingle(partialBindingDescription);
            if (bindingDescription == null)
                return null;

            bindingDescription.TargetName = targetPropertyName;
            var request = new CrossBindingRequest(source, target, bindingDescription);
            return BindSingle(request);
        }

        public ICrossUpdateableBinding BindSingle(CrossBindingRequest bindingRequest)
        {
            return new CrossFullBinding(bindingRequest);
        }
    }
}
