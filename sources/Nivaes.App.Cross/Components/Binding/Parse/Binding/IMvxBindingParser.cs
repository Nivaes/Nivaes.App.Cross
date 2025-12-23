namespace Nivaes.App.Cross
{
    public interface IMvxBindingParser
    {
        bool TryParseBindingDescription(string text, out MvxSerializableBindingDescription requestedDescription);

        bool TryParseBindingSpecification(string text, out MvxSerializableBindingSpecification requestedBindings);
    }
}
