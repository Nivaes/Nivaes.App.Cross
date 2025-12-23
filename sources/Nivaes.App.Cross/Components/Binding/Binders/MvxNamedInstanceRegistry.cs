namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public class MvxNamedInstanceRegistry<T>
        : IMvxNamedInstanceLookup<T>, ICrossNamedInstanceRegistry<T>
        where T : class
    {
        private readonly Dictionary<string, T> _converters =
            new Dictionary<string, T>();

        public T? Find(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (!_converters.TryGetValue(name, out var toReturn))
            {
                // no trace here - this is expected to fail sometimes - e.g. in the case where we look for first combiner, then converter
            }
            return toReturn;
        }

        public void AddOrOverwrite(string name, T instance)
        {
            _converters[name] = instance;
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        public void AddOrOverwriteFrom(Assembly assembly)
        {
            this.Fill(assembly);
        }
    }
}
