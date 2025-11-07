namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public interface ICrossBindingDescriptionParser
    {
        IEnumerable<CrossBindingDescription> Parse(string text);

        IEnumerable<CrossBindingDescription> LanguageParse(string text);

        CrossBindingDescription ParseSingle(string text);

        CrossBindingDescription SerializableBindingToBinding(string targetName, CrossSerializableBindingDescription description);
    }
}
