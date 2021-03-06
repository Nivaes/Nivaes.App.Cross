﻿namespace Nivaes.App.Cross.Desktop.Windows.Wpf.Sample
{
    using MvvmCross.Platforms.Wpf.Presenters.Attributes;
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    public partial class WindowChildView
        : IMvxOverridePresentationAttribute
    {
        public WindowChildView()
        {
            InitializeComponent();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            var instanceRequest = request as MvxViewModelInstanceRequest;
            var viewModel = instanceRequest?.ViewModelInstance as WindowChildViewModel;

            return new MvxContentPresentationAttribute
            {
                WindowIdentifier = $"{nameof(WindowView)}.{viewModel?.ParentNo}",
                StackNavigation = false
            };
        }
    }
}
