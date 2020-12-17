namespace Nivaes.App.Cross.Desktop.Windows.Wpf.Sample
{
    using MvvmCross.Platforms.Wpf.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    public partial class WindowView
        : IMvxOverridePresentationAttribute
    {
        public WindowView()
        {
            InitializeComponent();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            var instanceRequest = request as MvxViewModelInstanceRequest;
            var viewModel = instanceRequest?.ViewModelInstance as WindowViewModel;

            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(WindowView)}.{viewModel?.Count}"
            };
        }
    }
}
