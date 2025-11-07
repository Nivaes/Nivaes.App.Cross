namespace Nivaes.App.Cross
{
    public abstract class CrossValueCombiner
        : ICrossValueCombiner
    {
        public virtual Type SourceType(IEnumerable<ICrossSourceStep> steps)
        {
            return typeof(object);
        }

        public virtual void SetValue(IEnumerable<ICrossSourceStep> steps, object value)
        {
            // do nothing
        }

        public virtual bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value)
        {
            value = null;
            return false;
        }

        public virtual IEnumerable<Type> SubStepTargetTypes(IEnumerable<ICrossSourceStep> subSteps,
                                                            Type overallTargetType)
        {
            // by default a combiner just demand objects from its sources
            return subSteps.Select(x => typeof(object));
        }
    }
}
