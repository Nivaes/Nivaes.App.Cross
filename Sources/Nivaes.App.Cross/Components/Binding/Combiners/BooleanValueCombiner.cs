using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    [CrossValueCombiner(Name = "Inverted")]
    public class InvertedValueCombiner
            : BooleanValueCombiner
    {
        public InvertedValueCombiner(ILogger<InvertedValueCombiner> logger)
          : base(logger)
        {
        }

        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Exists(x => !x);
            return true;
        }
    }

    [CrossValueCombiner(Name = "And")]
    public class AndValueCombiner
        : BooleanValueCombiner
    {
        public AndValueCombiner(ILogger<AndValueCombiner> logger)
          : base(logger)
        {
        }

        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.TrueForAll(x => x);
            return true;
        }
    }

    [CrossValueCombiner(Name = "Or")]
    public class OrValueCombiner
        : BooleanValueCombiner
    {
        public OrValueCombiner(ILogger<OrValueCombiner> logger)
          : base(logger)
        {
        }

        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Exists(x => x);
            return true;
        }
    }

    [CrossValueCombiner(Name = "Not")]
    public class NotValueCombiner
        : BooleanValueCombiner
    {
        public NotValueCombiner(ILogger<CrossFormatValueCombiner> logger)
           : base(logger)
        {
        }

        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.TrueForAll(x => !x);
            return true;
        }
    }

    [CrossValueCombiner(Name = "XOr")]
    public class XorValueCombiner
        : BooleanValueCombiner
    {
        public XorValueCombiner(ILogger<XorValueCombiner> logger)
          : base(logger)
        {
        }

        protected override bool TryCombine(List<bool> stepValues, out object value)
        {
            value = stepValues.Exists(x => !x) &&
                    stepValues.Exists(x => x);
            return true;
        }
    }

    public abstract class BooleanValueCombiner
        : CrossValueCombiner
    {
        protected BooleanValueCombiner(ILogger logger)
            : base(logger)
        {
        }

        public override bool TryGetValue(
            IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var stepValues = new List<bool>();
            foreach (var step in steps)
            {
                var objectValue = step.GetValue();

                if (objectValue == CrossBindingConstant.DoNothing)
                {
                    value = CrossBindingConstant.DoNothing;
                    return true;
                }
                if (objectValue == CrossBindingConstant.UnsetValue)
                {
                    value = CrossBindingConstant.UnsetValue;
                    return true;
                }
                if (!TryConvertToBool(objectValue, out var booleanValue))
                {
                    value = CrossBindingConstant.UnsetValue;
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
