namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [Obsolete("", true)]
    public class MvxCachedViewTypeResolver 
        : IMvxViewTypeResolver
    {
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>();
        private readonly IMvxViewTypeResolver _resolver;

        public MvxCachedViewTypeResolver(IMvxViewTypeResolver resolver)
        {
            _resolver = resolver;
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type Resolve(string tagName)
        {
            if (_cache.TryGetValue(tagName, out Type? toReturn))
                return toReturn;

            toReturn = _resolver.Resolve(tagName);
            _cache[tagName] = toReturn;
            return toReturn;
        }
    }
}
