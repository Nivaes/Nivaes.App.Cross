namespace MvvmCross.Platforms.Android.Binding.Target
{
    using Nivaes.App.Cross;

    public class MvxNumberPickerDisplayedValuesTargetBinding(NumberPicker target)
        : MvxTargetBinding<NumberPicker, IEnumerable<string>?>(target)
    {
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        protected override void SetValue(IEnumerable<string>? value)
        {
            if (Target == null)
                return;

            var arrayVal = value?.ToArray() ?? [];

            if (Target.MaxValue == 0)
                Target.MaxValue = arrayVal.Length - 1;
            Target.SetDisplayedValues(arrayVal);
            Target.Invalidate();
        }
    }
}