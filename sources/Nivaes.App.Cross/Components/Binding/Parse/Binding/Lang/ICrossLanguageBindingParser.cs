namespace Nivaes.App.Cross
{
    public interface ICrossLanguageBindingParser
        : ICrossBindingParser
    {
        string DefaultConverterName { get; set; }
        string DefaultTextSourceName { get; set; }
    }
}
