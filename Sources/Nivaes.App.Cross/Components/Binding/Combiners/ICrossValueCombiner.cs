using System;
using System.Collections.Generic;

namespace Nivaes.App.Cross
{
    public interface ICrossValueCombiner
    {
        Type SourceType(IEnumerable<ICrossSourceStep> steps);

        void SetValue(IEnumerable<ICrossSourceStep> steps, object? value);

        bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object? value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<ICrossSourceStep> subSteps, Type overallTargetType);
    }
}
