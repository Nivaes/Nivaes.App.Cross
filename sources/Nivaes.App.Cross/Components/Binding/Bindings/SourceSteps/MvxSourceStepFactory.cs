namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public class MvxSourceStepFactory : IMvxSourceStepFactoryRegistry
    {
        private readonly Dictionary<Type, IMvxSourceStepFactory> _subFactories =
            new Dictionary<Type, IMvxSourceStepFactory>();

        public void AddOrOverwrite(Type type, IMvxSourceStepFactory factory)
        {
            _subFactories[type] = factory;
        }

        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        public IMvxSourceStep Create(MvxSourceStepDescription description)
        {
            if (!_subFactories.TryGetValue(description.GetType(), out IMvxSourceStepFactory? subFactory))
            {
                throw new CrossException("Failed to get factory for step type {0}", description.GetType().Name);
            }

            return subFactory.Create(description);
        }
    }
}
