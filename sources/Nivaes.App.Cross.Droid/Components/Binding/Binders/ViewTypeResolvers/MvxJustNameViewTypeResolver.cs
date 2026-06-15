namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.Views;
    using MvvmCross.IoC;

    [Obsolete("", true)]
    public class MvxJustNameViewTypeResolver : MvxReflectionViewTypeResolver
    {
        public MvxJustNameViewTypeResolver(IMvxTypeCache typeCache) : base(typeCache)
        {
        }

        [return: System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)]
        public override Type? Resolve(string tagName)
        {
            // this resolver can't handle fully qualified tag names
            if (IsFullyQualified(tagName))
                return null;

            TypeCache.NameCache.TryGetValue(tagName, out Type? toReturn);
            return toReturn;
        }
    }
}
