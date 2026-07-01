namespace Nivaes.App.Cross
{
    public interface ICrossBindingParser
    {
        bool TryParseBindingDescription(string text, out CrossSerializableBindingDescription requestedDescription);

        bool TryParseBindingSpecification(string text, out CrossSerializableBindingSpecification requestedBindings);
    }
}
