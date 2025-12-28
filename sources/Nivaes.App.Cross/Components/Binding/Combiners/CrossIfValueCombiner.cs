using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

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
        if (result == CrossBindingConstant.DoNothing)
        {
            value = CrossBindingConstant.DoNothing;
            return true;
        }

        if (result == CrossBindingConstant.UnsetValue)
        {
            value = CrossBindingConstant.UnsetValue;
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
            return CrossBindingConstant.UnsetValue;
        }
        return subStep.GetValue();
    }
}
