namespace Nivaes.App.Cross
{
    using Nivaes.IoC;

    public static class ViewModelLoader
    {
        public static TViewModel? LoadViewModel<TViewModel>()
             where TViewModel : IViewModel
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            var viewModel = container.Resolve<TViewModel>();

            return viewModel;
        }
    }
}
