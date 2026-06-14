namespace Nivaes.App.Cross
{
    public interface ICrossOverridePresentationAttribute
    {
        CrossBasePresentationAttribute? PresentationAttribute(CrossViewModelRequest request);
    }
}
