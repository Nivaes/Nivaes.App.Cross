namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;

    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<IMvxSourceStep> steps);

        void SetValue(IEnumerable<IMvxSourceStep> steps, object value);

        bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, Type overallTargetType);
    }
}
