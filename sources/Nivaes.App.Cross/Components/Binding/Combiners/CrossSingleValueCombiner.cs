namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CrossSingleValueCombiner 
        : CrossValueCombiner
    {
        public override Type SourceType(IEnumerable<ICrossSourceStep> steps)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
                return typeof(object);

            return firstStep.SourceType;
        }

        public override void SetValue(IEnumerable<ICrossSourceStep> steps, object value)
        {
            var firstStep = steps.FirstOrDefault();

            firstStep?.SetValue(value);
        }

        public override bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
            {
                value = CrossBindingConstant.UnsetValue;
                return true;
            }

            value = firstStep.GetValue();
            return true;
        }
    }
}
