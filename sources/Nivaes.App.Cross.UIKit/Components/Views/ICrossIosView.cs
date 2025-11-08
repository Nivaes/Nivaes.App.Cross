namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossIosView
        : ICrossView
        , ICrossCanCreateIosView
        , ICrossBindingContextOwner
    {
        ICrossViewModelRequest Request { get; set; }
    }

    public interface ICrossIosView<TViewModel>
        : ICrossIosView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<ICrossIosView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
