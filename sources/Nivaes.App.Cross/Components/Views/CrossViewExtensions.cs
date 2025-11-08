namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    public static class CrossViewExtensions
    {
        public static void OnViewCreate(this ICrossView view, Func<ICrossViewModel?> viewModelLoader)
        {
            // note - we check the DataContent before the ViewModel to avoid casting errors
            //       in the case of 'simple' binding code
            if (view.DataContext != null)
                return;

            if (view.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            if (viewModel == null)
            {
                CrossLogHost.Default?.Log(LogLevel.Warning, "ViewModel not loaded for view {ViewTypeName}", view.GetType().Name);
                return;
            }

            view.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this ICrossView view)
        {
            // nothing needed currently
        }

        public static Type? FindAssociatedViewModelTypeOrNull<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TViewType>(
            this TViewType view)
                where TViewType : ICrossView
        {
            ArgumentNullException.ThrowIfNull(view);

            if (Mvx.IoCProvider?.TryResolve(out ICrossViewModelTypeFinder? associatedTypeFinder) == true)
                return associatedTypeFinder?.FindTypeOrNull(typeof(TViewType));

            CrossLogHost.Default?.Log(LogLevel.Trace,
                "No view model type finder available - assuming we are looking for a splash screen - returning null");
            return typeof(CrossNullViewModel);
        }

        public static ICrossBundle CreateSaveStateBundle(this ICrossView view)
        {
            var viewModel = view.ViewModel;
            return viewModel == null ? new CrossBundle() : viewModel.SaveStateBundle();
        }
    }
}