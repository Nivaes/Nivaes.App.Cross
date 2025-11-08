namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class CrossCompositeViewTypeResolver : ICrossViewTypeResolver
    {
        private readonly List<ICrossViewTypeResolver> _resolvers;

        public CrossCompositeViewTypeResolver(params ICrossViewTypeResolver[] resolvers)
        {
            _resolvers = new List<ICrossViewTypeResolver>(resolvers);
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? Resolve(string tagName)
        {
            foreach (var resolver in _resolvers)
            {
                var result = resolver.Resolve(tagName);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
