namespace Nivaes.App.Cross
{
    public class MvxInvertedValueCombiner
            : CrossBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Exists(x => !x);
            return true;
        }
    }

    public class MvxAndValueCombiner
        : CrossBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.TrueForAll(x => x);
            return true;
        }
    }

    public class MvxOrValueCombiner
        : CrossBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Exists(x => x);
            return true;
        }
    }

    public class MvxNotValueCombiner
        : CrossBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.TrueForAll(x => !x);
            return true;
        }
    }

    public class MvxXorValueCombiner
        : CrossBooleanValueCombiner
    {
        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Exists(x => !x) &&
                    stepValues.Exists(x => x);
            return true;
        }
    }

    public abstract class CrossBooleanValueCombiner
        : CrossValueCombiner
    {
        public override bool TryGetValue(
            IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var stepValues = new List<bool>();
            foreach (var step in steps)
            {
                var objectValue = step.GetValue();

                if (objectValue == MvxBindingConstant.DoNothing)
                {
                    value = MvxBindingConstant.DoNothing;
                    return true;
                }
                if (objectValue == MvxBindingConstant.UnsetValue)
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }
                if (!TryConvertToBool(objectValue, out var booleanValue))
                {
                    value = MvxBindingConstant.UnsetValue;
                    return true;
                }
                stepValues.Add(booleanValue);
            }

            return TryCombine(stepValues, out value);
        }

        protected abstract bool TryCombine(List<bool> stepValues, out object value);

        protected virtual bool TryConvertToBool(object objectValue, out bool booleanValue)
        {
            booleanValue = objectValue.ConvertToBoolean();
            return true;
        }
    }
}
