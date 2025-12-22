namespace Nivaes.App.Cross
{
    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Converters;

    public class CrossBindingDescription
    {
        public CrossBindingDescription()
        {
        }

        public CrossBindingDescription(string targetName, string sourcePropertyPath, IMvxValueConverter converter,
                                     object converterParameter, object fallbackValue, MvxBindingMode mode)
        {
            TargetName = targetName;
            Mode = mode;
            Source = new MvxPathSourceStepDescription
            {
                SourcePropertyPath = sourcePropertyPath,
                Converter = converter,
                ConverterParameter = converterParameter,
                FallbackValue = fallbackValue,
            };
        }

        public string TargetName { get; set; }
        public MvxBindingMode Mode { get; set; }
        public MvxSourceStepDescription Source { get; set; }

        public override string ToString()
        {
            return $"binding {TargetName} for {(Source == null ? "-null" : Source.ToString())}";
        }
    }
}
