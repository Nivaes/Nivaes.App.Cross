namespace Nivaes.App.Cross
{
    public interface IMvxLanguageBindingParser
        : IMvxBindingParser
    {
        string DefaultConverterName { get; set; }
        string DefaultTextSourceName { get; set; }
    }
}
