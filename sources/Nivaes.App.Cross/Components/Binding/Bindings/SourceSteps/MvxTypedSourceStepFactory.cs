namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class MvxTypedSourceStepFactory<T>
        : IMvxSourceStepFactory
        where T : MvxSourceStepDescription
    {
        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            return TypedCreate((T)description);
        }

        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        protected abstract IMvxSourceStep TypedCreate(T description);
    }
}
