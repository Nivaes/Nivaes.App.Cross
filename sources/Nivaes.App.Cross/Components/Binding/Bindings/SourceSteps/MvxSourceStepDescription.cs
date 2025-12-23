namespace Nivaes.App.Cross
{
    public class MvxSourceStepDescription
    {
        public IMvxValueConverter? Converter { get; set; }
        public object? ConverterParameter { get; set; }
        public object? FallbackValue { get; set; }
    }
}
