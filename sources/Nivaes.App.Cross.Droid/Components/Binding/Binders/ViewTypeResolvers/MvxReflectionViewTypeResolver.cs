namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.IoC;

    [Obsolete("", true)]
    public abstract class MvxReflectionViewTypeResolver : IMvxViewTypeResolver
    {
        protected IMvxTypeCache TypeCache { get; }

        protected MvxReflectionViewTypeResolver(IMvxTypeCache typeCache)
        {
            TypeCache = typeCache;
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public abstract Type? Resolve(string tagName);

        protected static bool IsFullyQualified(string tagName)
        {
            return tagName.Contains(".");
        }
    }
}
