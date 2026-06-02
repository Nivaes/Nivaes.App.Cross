namespace Nivaes.App.Cross
{
    [Obsolete("Use ICrossViewModelLocator instead", true)]
    internal interface ICrossViewModelLocatorCollection
    {
        ICrossViewModelLocator FindViewModelLocator(CrossViewModelRequest request);
    }
}
