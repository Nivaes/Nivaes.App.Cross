namespace MvvmCross.Platforms.Ios.Views
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public static class MvxCanCreateIosViewExtensions
    {
        public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
                this IMvxCanCreateIosView view,
                object parameterObject)
            where TTargetViewModel : class, ICrossViewModel =>
            view.CreateViewControllerFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());

        // TODO - could this move down to IMvxView level?
        public static IMvxIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this IMvxCanCreateIosView view,
            IDictionary<string, string>? parameterValues = null)
            where TTargetViewModel : class, ICrossViewModel
        {
            var parameterBundle = new CrossBundle(parameterValues);
            var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxIosView? CreateViewControllerFor(
            this IMvxCanCreateIosView view,
            CrossViewModelRequest request)
        {
            return Mvx.IoCProvider?.Resolve<IMvxIosViewCreator>()?.CreateView(request);
        }

        public static IMvxIosView? CreateViewControllerFor(
            this IMvxCanCreateIosView view, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType)
        {
            return Mvx.IoCProvider?.Resolve<IMvxIosViewCreator>()?.CreateViewOfType(viewType);
        }

        public static IMvxIosView? CreateViewControllerFor(
            this IMvxCanCreateIosView view,
            ICrossViewModel viewModel)
        {
            return Mvx.IoCProvider?.Resolve<IMvxIosViewCreator>()?.CreateView(viewModel);
        }
    }
}