namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MvxSingleValueCombiner 
        : MvxValueCombiner
    {
        public override Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
                return typeof(object);

            return firstStep.SourceType;
        }

        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            var firstStep = steps.FirstOrDefault();

            firstStep?.SetValue(value);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            value = firstStep.GetValue();
            return true;
        }
    }
}
