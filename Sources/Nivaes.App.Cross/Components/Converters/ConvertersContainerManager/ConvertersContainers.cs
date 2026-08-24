namespace Nivaes.App.Cross;

public class ConvertersContainers
{
    public Dictionary<string, ICrossValueConverter> NameConverters { get; } = new Dictionary<string, ICrossValueConverter>();

    public Dictionary<Type, ICrossValueConverter> Converters { get; } = new Dictionary<Type, ICrossValueConverter>();
}
