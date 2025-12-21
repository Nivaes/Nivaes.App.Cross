namespace Nivaes.App.Cross
{
    public interface ICrossViewModelLocatorCollection
    {
        ICrossViewModelLocator FindViewModelLocator(CrossViewModelRequest request);
    }
}
