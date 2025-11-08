namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class CrossCachedViewTypeResolver : ICrossViewTypeResolver
    {
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>();
        private readonly ICrossViewTypeResolver _resolver;

        public CrossCachedViewTypeResolver(ICrossViewTypeResolver resolver)
        {
            _resolver = resolver;
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type Resolve(string tagName)
        {
            Type toReturn;
            if (_cache.TryGetValue(tagName, out toReturn))
                return toReturn;

            toReturn = _resolver.Resolve(tagName);
            _cache[tagName] = toReturn;
            return toReturn;
        }
    }
}
