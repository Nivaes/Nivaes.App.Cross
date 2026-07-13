namespace Nivaes.App.Cross
{
    [Obsolete("No usar Override", true)]
    public interface ICrossOverridePresentationAttribute
    {
        CrossBasePresentationAttribute? PresentationAttribute(CrossViewModelRequest request);
    }
}
