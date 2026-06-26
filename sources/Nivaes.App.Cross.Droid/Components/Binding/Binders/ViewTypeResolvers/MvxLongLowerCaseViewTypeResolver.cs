namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.IoC;

    [Obsolete("", true)]
    public abstract class MvxLongLowerCaseViewTypeResolver
        : MvxReflectionViewTypeResolver
    {
        protected MvxLongLowerCaseViewTypeResolver(IMvxTypeCache typeCache)
            : base(typeCache)
        {
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        protected Type? ResolveLowerCaseTypeName(string longLowerCaseName)
        {
            TypeCache.LowerCaseFullNameCache.TryGetValue(longLowerCaseName, out Type? toReturn);
            return toReturn;
        }
    }
}
