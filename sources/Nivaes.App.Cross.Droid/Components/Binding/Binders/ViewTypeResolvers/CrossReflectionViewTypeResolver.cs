namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.IoC;

    public abstract class CrossReflectionViewTypeResolver : ICrossViewTypeResolver
    {
        protected IMvxTypeCache TypeCache { get; }

        protected CrossReflectionViewTypeResolver(IMvxTypeCache typeCache)
        {
            TypeCache = typeCache;
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public abstract Type Resolve(string tagName);

        protected static bool IsFullyQualified(string tagName)
        {
            return tagName.Contains(".");
        }
    }
}
