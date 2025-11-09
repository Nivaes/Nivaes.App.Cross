namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;

    public static class CrossCanCreateIosViewExtensions
    {
        public static ICrossIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this ICrossCanCreateIosView view,
            object parameterObject)
        where TTargetViewModel : class, ICrossViewModel =>
        view.CreateViewControllerFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());

        // TODO - could this move down to IMvxView level?
        public static ICrossIosView? CreateViewControllerFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
            this ICrossCanCreateIosView view,
            IDictionary<string, string>? parameterValues = null)
            where TTargetViewModel : class, ICrossViewModel
        {
            var parameterBundle = new CrossBundle(parameterValues);
            var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateViewControllerFor(request);
        }

        public static ICrossIosView? CreateViewControllerFor(
            this ICrossCanCreateIosView view,
            ICrossViewModelRequest request)
        {
            throw new NotImplementedException();
            //return Mvx.IoCProvider?.Resolve<ICrossIosViewCreator>()?.CreateView(request);
        }

        public static ICrossIosView? CreateViewControllerFor(
            this ICrossCanCreateIosView view, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType)
        {
            throw new NotImplementedException();
            //return Mvx.IoCProvider?.Resolve<ICrossIosViewCreator>()?.CreateViewOfType(viewType);
        }

        public static ICrossIosView? CreateViewControllerFor(
            this ICrossCanCreateIosView view,
            ICrossViewModel viewModel)
        {
            throw new NotImplementedException();
            //return Mvx.IoCProvider?.Resolve<ICrossIosViewCreator>()?.CreateView(viewModel);
        }
    }
}