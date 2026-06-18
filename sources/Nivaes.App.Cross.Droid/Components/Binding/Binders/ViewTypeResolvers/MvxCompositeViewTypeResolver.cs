namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class MvxCompositeViewTypeResolver : IMvxViewTypeResolver
    {
        private readonly List<IMvxViewTypeResolver> _resolvers;

        [Obsolete("", true)]
        public MvxCompositeViewTypeResolver(params IMvxViewTypeResolver[] resolvers)
        {
            _resolvers = new List<IMvxViewTypeResolver>(resolvers);
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type Resolve(string tagName)
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
