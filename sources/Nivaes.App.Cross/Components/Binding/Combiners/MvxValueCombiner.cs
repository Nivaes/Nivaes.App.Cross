namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class MvxValueCombiner
        : IMvxValueCombiner
    {
        public virtual Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            return typeof(object);
        }

        public virtual void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            // do nothing
        }

        public virtual bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            value = null;
            return false;
        }

        public virtual IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps,
                                                            Type overallTargetType)
        {
            // by default a combiner just demand objects from its sources
            return subSteps.Select(x => typeof(object));
        }
    }
}
