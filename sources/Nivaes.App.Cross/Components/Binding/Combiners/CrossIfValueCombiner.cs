namespace MvvmCross.Binding.Combiners
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class CrossIfValueCombiner
        : CrossValueCombiner
    {
        public override bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var list = steps.ToList();
            switch (list.Count)
            {
                case 2:
                    return TryEvaluateif(list[0], list[1], null, out value);

                case 3:
                    return TryEvaluateif(list[0], list[1], list[2], out value);

                default:
                    CrossBindingLog.Instance?.LogWarning("Unexpected substep count of {Count} in 'If' ValueCombiner", list.Count);
                    return base.TryGetValue(list, out value);
            }
        }

        private bool TryEvaluateif(ICrossSourceStep testStep, ICrossSourceStep ifStep, ICrossSourceStep? elseStep, out object value)
        {
            var result = testStep.GetValue();
            if (result == MvxBindingConstant.DoNothing)
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (result == MvxBindingConstant.UnsetValue)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            if (IsTrue(result))
            {
                value = ReturnSubStepResult(ifStep);
                return true;
            }

            value = ReturnSubStepResult(elseStep);
            return true;
        }

        protected virtual bool IsTrue(object result)
        {
            return result.ConvertToBoolean();
        }

        protected virtual object ReturnSubStepResult(ICrossSourceStep? subStep)
        {
            if (subStep == null)
            {
                return MvxBindingConstant.UnsetValue;
            }
            return subStep.GetValue();
        }
    }
}
