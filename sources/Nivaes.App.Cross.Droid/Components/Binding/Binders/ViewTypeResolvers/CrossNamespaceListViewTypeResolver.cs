namespace Nivaes.App.Cross.Droid
{
    using MvvmCross.IoC;

    public class CrossNamespaceListViewTypeResolver : 
        CrossLongLowerCaseViewTypeResolver, ICrossNamespaceListViewTypeResolver
    {
        public IList<string> Namespaces { get; }

        public CrossNamespaceListViewTypeResolver(IMvxTypeCache typeCache)
            : base(typeCache)
        {
            Namespaces = new List<string>();
        }

        public void Add(string namespaceName)
        {
            namespaceName = namespaceName.ToLower();
            if (!namespaceName.EndsWith('.'))
                namespaceName += '.';

            Namespaces.Add(namespaceName);
        }

        [return: System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)]
        public override Type? Resolve(string tagName)
        {
            // this resolver only handles simple namespaceless tagNames
            if (tagName.Contains('.'))
                return null;

            var lowerTagName = tagName.ToLower();
            foreach (var ns in Namespaces)
            {
                var candidateName = ns + lowerTagName;
                if (TypeCache.LowerCaseFullNameCache.TryGetValue(candidateName, out var type))
                    return type;
            }

            return null;
        }
    }
}
