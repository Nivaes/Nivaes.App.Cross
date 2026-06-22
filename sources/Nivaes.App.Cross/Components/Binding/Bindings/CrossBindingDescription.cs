using System.Diagnostics;

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

        public string? TargetName { [DebuggerHidden]get; [DebuggerHidden]set; }
        public CrossBindingMode Mode { [DebuggerHidden]get; [DebuggerHidden]set; }
        public CrossSourceStepDescription? Source { [DebuggerHidden]get; [DebuggerHidden]set; }

        public override string ToString()
        {
            return $"binding {TargetName} for {(Source == null ? "-null" : Source.ToString())}";
        }
    }
}
