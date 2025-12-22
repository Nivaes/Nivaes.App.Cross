namespace MvvmCross.Binding.Binders
{
    using System.Collections.Generic;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Binding.Parse.Binding;
    using Nivaes.App.Cross;

    public interface IMvxBindingDescriptionParser
    {
        IEnumerable<CrossBindingDescription> Parse(string text);

        IEnumerable<CrossBindingDescription> LanguageParse(string text);

        CrossBindingDescription ParseSingle(string text);

        CrossBindingDescription SerializableBindingToBinding(string targetName,
                                                           MvxSerializableBindingDescription description);
    }
}
