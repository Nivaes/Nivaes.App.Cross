namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [Obsolete("No compatible con AoT", true)]
    public class CrossNamedInstanceRegistry<T>
        : ICrossNamedInstanceLookup<T>, ICrossNamedInstanceRegistry<T>
        where T : class
    {
        private readonly Dictionary<string, T> _converters =
            new Dictionary<string, T>();

        public T Find(string name)
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
