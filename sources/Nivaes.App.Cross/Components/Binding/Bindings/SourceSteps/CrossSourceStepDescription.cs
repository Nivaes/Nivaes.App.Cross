namespace Nivaes.App.Cross
{
    public class CrossSourceStepDescription
    {
        public IMvxValueConverter? Converter { get; set; }
        public object? ConverterParameter { get; set; }
        public object? FallbackValue { get; set; }
    }
}
