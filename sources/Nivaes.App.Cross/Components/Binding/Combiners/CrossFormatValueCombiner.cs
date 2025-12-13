namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    public class CrossFormatValueCombiner : CrossValueCombiner
    {
        public override bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var list = steps.ToList();

            if (list.Count < 1)
            {
                CrossBindingLog.Instance?.LogWarning("Format called with no parameters - will fail");
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

            var formatString = formatObject == null ? string.Empty : formatObject.ToString();

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
