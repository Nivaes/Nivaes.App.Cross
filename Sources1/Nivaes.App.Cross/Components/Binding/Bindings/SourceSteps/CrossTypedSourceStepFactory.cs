namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class CrossTypedSourceStepFactory<T>
        : ICrossSourceStepFactory
        where T : CrossSourceStepDescription
    {
        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        public ICrossSourceStep Create(CrossSourceStepDescription description)
        {
            return TypedCreate((T)description);
        }

        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected abstract ICrossSourceStep TypedCreate(T description);
    }
}
