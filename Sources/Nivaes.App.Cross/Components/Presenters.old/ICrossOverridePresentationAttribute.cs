namespace Nivaes.App.Cross
{
    [Obsolete("No usar Override", true)]
    public interface ICrossOverridePresentationAttribute
    {
        BasePresentationAttribute? PresentationAttribute(CrossViewModelRequest request);
    }
}
