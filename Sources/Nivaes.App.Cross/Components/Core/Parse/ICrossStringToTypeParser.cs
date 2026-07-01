namespace Nivaes.App.Cross
{
    public interface ICrossStringToTypeParser
    {
        bool TypeSupported(Type targetType);

        object? ReadValue(string rawValue, Type targetType, string fieldOrParameterName);
    }
}
