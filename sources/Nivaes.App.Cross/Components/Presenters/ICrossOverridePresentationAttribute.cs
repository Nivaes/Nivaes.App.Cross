namespace Nivaes.App.Cross
{
    using MvvmCross.Presenters.Attributes;
    using MvvmCross.ViewModels;

    public interface ICrossOverridePresentationAttribute
    {
        MvxBasePresentationAttribute PresentationAttribute(CrossViewModelRequest request);
    }
}
