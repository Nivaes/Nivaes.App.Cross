namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossStringToTypeParser
    {
        bool TypeSupported(Type targetType);

        object? ReadValue(string rawValue, Type targetType, string fieldOrParameterName);
    }
}
