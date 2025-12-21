namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossCommandCollectionBuilder
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        ICrossCommandCollection BuildCollectionFor(object owner);
    }
}
