namespace Nivaes.App
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class ClassDeclarationSyntaxExtensions
    {
        private const string NESTED_CLASS_DELIMITER = "+";
        private const string NAMESPACE_CLASS_DELIMITER = ".";

        internal static string GetFullName(this ClassDeclarationSyntax source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var items = new List<string>();
            var parent = source.Parent;
            while (parent.IsKind(SyntaxKind.ClassDeclaration))
            {
                var parentClass = (ClassDeclarationSyntax)parent;
                items.Add(parentClass.Identifier.Text);

                parent = parent.Parent;
            }

            var nameSpace = (NamespaceDeclarationSyntax?)parent;
            
            var sb = new StringBuilder().Append(nameSpace.Name).Append(NAMESPACE_CLASS_DELIMITER);
            items.Reverse();
            items.ForEach(i => { sb.Append(i).Append(NESTED_CLASS_DELIMITER); });
            sb.Append(source.Identifier.Text);

            var result = sb.ToString();
            return result;
        }
    }
}
