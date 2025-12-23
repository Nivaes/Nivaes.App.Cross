namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;

    public interface IMvxValueCombiner
    {
        Type SourceType(IEnumerable<ICrossSourceStep> steps);

        void SetValue(IEnumerable<ICrossSourceStep> steps, object value);

        bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<ICrossSourceStep> subSteps, Type overallTargetType);
    }
}
