namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;

    public class CrossFormatValueCombiner : CrossValueCombiner
    {
        public override bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var list = steps.ToList();

            if (list.Count < 1)
            {
                CrossBindingLog.Instance?.LogWarning("Format called with no parameters - will fail");
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            var formatObject = list[0].GetValue();
            if (formatObject == MvxBindingConstant.DoNothing)
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (formatObject == MvxBindingConstant.UnsetValue)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            var formatString = formatObject == null ? string.Empty : formatObject.ToString();

            var values = list.Skip(1).Select(s => s.GetValue()).ToArray();

            if (Array.Exists(values, v => v == MvxBindingConstant.DoNothing))
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (Array.Exists(values, v => v == MvxBindingConstant.UnsetValue))
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            value = string.Format(formatString, values);
            return true;
        }
    }
}
