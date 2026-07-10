using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    [CrossValueCombiner(Name = "Format")]
    public sealed class CrossFormatValueCombiner : CrossValueCombiner
    {
        public CrossFormatValueCombiner(ILogger<CrossFormatValueCombiner> logger)
            :base(logger)
        {
        }

        public override bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var list = steps.ToList();

            if (list.Count < 1)
            {
                Logger.LogWarning("Format called with no parameters - will fail");
                value = CrossBindingConstant.DoNothing;
                return true;
            }

            var formatObject = list[0].GetValue();
            if (formatObject == CrossBindingConstant.DoNothing)
            {
                value = CrossBindingConstant.DoNothing;
                return true;
            }

            if (formatObject == CrossBindingConstant.UnsetValue)
            {
                value = CrossBindingConstant.UnsetValue;
                return true;
            }

            var formatString = formatObject?.ToString() ?? string.Empty;

            var values = list.Skip(1).Select(s => s.GetValue()).ToArray();

            if (Array.Exists(values, v => v == CrossBindingConstant.DoNothing))
            {
                value = CrossBindingConstant.DoNothing;
                return true;
            }

            if (Array.Exists(values, v => v == CrossBindingConstant.UnsetValue))
            {
                value = CrossBindingConstant.UnsetValue;
                return true;
            }

            value = string.Format(formatString, values);
            return true;
        }
    }
}
