namespace Nivaes.App.Cross
{
    public class CrossBindingDescription
    {
        public CrossBindingDescription()
        {
        }

        public CrossBindingDescription(string targetName, string sourcePropertyPath, ICrossValueConverter converter,
                                     object converterParameter, object fallbackValue, CrossBindingMode mode)
        {
            TargetName = targetName;
            Mode = mode;
            Source = new CrossPathSourceStepDescription
            {
                SourcePropertyPath = sourcePropertyPath,
                Converter = converter,
                ConverterParameter = converterParameter,
                FallbackValue = fallbackValue,
            };
        }

        public string TargetName { get; set; }
        public CrossBindingMode Mode { get; set; }
        public CrossSourceStepDescription Source { get; set; }

        public override string ToString()
        {
            return $"binding {TargetName} for {(Source == null ? "-null" : Source.ToString())}";
        }
    }
}
