namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public class CrossSerializableBindingDescription
    {
        public string? Converter { get; set; }
        public object? ConverterParameter { get; set; }
        public object? FallbackValue { get; set; }
        public CrossBindingMode Mode { get; set; }
        public IList<CrossSerializableBindingDescription>? Sources 
        { 
            get;
            set; 
        }

        public string? Function 
        { 
            get; 
            set;
        }

        public object? Literal { get; set; }
        public string? Path { get; set; }
    }
}
