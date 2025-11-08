namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.IoC;

    public abstract class CrossLongLowerCaseViewTypeResolver : CrossReflectionViewTypeResolver
    {
        protected CrossLongLowerCaseViewTypeResolver(IMvxTypeCache typeCache)
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
