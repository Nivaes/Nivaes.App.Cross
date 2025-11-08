namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.IoC;

    public class CrossJustNameViewTypeResolver : CrossReflectionViewTypeResolver
    {
        public CrossJustNameViewTypeResolver(IMvxTypeCache typeCache) : base(typeCache)
        {
        }

        [return: DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)]
        public override Type? Resolve(string tagName)
        {
            // this resolver can't handle fully qualified tag names
            if (IsFullyQualified(tagName))
                return null;

            Type toReturn;
            TypeCache.NameCache.TryGetValue(tagName, out toReturn);
            return toReturn;
        }
    }
}
