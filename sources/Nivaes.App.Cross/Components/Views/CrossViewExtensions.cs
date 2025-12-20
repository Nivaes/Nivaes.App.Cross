namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Logging;
    using MvvmCross.ViewModels;

    public static class CrossViewExtensions
    {
        public static void OnViewCreate(this ICrossView view, Func<IMvxViewModel?> viewModelLoader)
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
                MvxLogHost.Default?.Log(LogLevel.Warning, "ViewModel not loaded for view {ViewTypeName}", view.GetType().Name);
                return;
            }

            view.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this ICrossView view)
        {
            // nothing needed currently
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "The generic constraint ensures TViewType has the required members")]
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
        public static Type? FindAssociatedViewModelTypeOrNull<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TViewType>(
                this TViewType view)
            where TViewType : ICrossView
        {
            ArgumentNullException.ThrowIfNull(view);

            if (Mvx.IoCProvider?.TryResolve(out IMvxViewModelTypeFinder? associatedTypeFinder) == true)
                return associatedTypeFinder?.FindTypeOrNull(view.GetType());

            MvxLogHost.Default?.Log(LogLevel.Trace,
                "No view model type finder available - assuming we are looking for a splash screen - returning null");
            return typeof(MvxNullViewModel);
        }

        public static IMvxBundle CreateSaveStateBundle(this ICrossView view)
        {
            var viewModel = view.ViewModel;
            return viewModel == null ? new MvxBundle() : viewModel.SaveStateBundle();
        }
    }
}