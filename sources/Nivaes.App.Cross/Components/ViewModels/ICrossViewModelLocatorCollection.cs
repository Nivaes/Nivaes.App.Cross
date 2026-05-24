namespace Nivaes.App.Cross
{
    [Obsolete]
    public interface ICrossViewModelLocatorCollection
    {
        ICrossViewModelLocator FindViewModelLocator(CrossViewModelRequest request);
    }
}
