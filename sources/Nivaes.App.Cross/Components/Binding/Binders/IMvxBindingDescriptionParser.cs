namespace MvvmCross.Binding.Binders
{
    using System.Collections.Generic;
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
