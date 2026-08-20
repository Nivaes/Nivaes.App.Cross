namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossSourceBindingFactory
    {
        ICrossSourceBinding? CreateBinding(object source, string combinedPropertyName);

        ICrossSourceBinding? CreateBinding(object source, IList<ICrossPropertyToken>? tokens);
    }
}
