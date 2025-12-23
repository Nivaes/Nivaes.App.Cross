namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;
    using MvvmCross.IoC;

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
