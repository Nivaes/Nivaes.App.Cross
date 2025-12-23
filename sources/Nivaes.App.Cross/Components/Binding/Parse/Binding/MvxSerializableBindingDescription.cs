namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public class MvxSerializableBindingDescription
    {
        public string? Converter { get; set; }
        public object? ConverterParameter { get; set; }
        public object? FallbackValue { get; set; }
        public MvxBindingMode Mode { get; set; }
        public IList<MvxSerializableBindingDescription>? Sources { get; set; }
        public string? Function { get; set; }
        public object? Literal { get; set; }
        public string? Path { get; set; }
    }
}
