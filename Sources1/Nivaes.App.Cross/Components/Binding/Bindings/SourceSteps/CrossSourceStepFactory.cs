namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossSourceStepFactory : ICrossSourceStepFactoryRegistry
    {
        private readonly Dictionary<Type, ICrossSourceStepFactory> _subFactories =
            new Dictionary<Type, ICrossSourceStepFactory>();

        public void AddOrOverwrite(Type type, ICrossSourceStepFactory factory)
        {
            _subFactories[type] = factory;
        }

        [RequiresUnreferencedCode("This method creates source steps that may use type inspection which may not be preserved by trimming")]
        public ICrossSourceStep Create(CrossSourceStepDescription description)
        {
            if (!_subFactories.TryGetValue(description.GetType(), out ICrossSourceStepFactory? subFactory))
            {
                throw new CrossException("Failed to get factory for step type {0}", description.GetType().Name);
            }

            return subFactory.Create(description);
        }
    }
}
