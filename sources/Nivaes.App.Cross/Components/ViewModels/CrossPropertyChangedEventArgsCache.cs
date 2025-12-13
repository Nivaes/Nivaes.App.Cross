namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public sealed class CrossPropertyChangedEventArgsCache
    {
        /// <summary>
        /// The underlying dictionary. This instance is its own mutex.
        /// </summary>
        private readonly Dictionary<string, PropertyChangedEventArgs> _cache = new Dictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// Private constructor to prevent other instances.
        /// </summary>
        private CrossPropertyChangedEventArgsCache()
        {
        }

        /// <summary>
        /// The global instance of the cache.
        /// </summary>
        public static CrossPropertyChangedEventArgsCache Instance { get; } = new CrossPropertyChangedEventArgsCache();

        /// <summary>
        /// Retrieves a <see cref="PropertyChangedEventArgs"/> instance for the specified property, creating it and adding it to the cache if necessary.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedEventArgs Get(string propertyName)
        {
            lock (_cache)
            {
                PropertyChangedEventArgs? result;
                if (_cache.TryGetValue(propertyName, out result))
                    return result;
                result = new PropertyChangedEventArgs(propertyName);
                _cache.Add(propertyName, result);
                return result;
            }
        }
    }
}
