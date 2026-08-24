namespace Nivaes.App.Cross
{
    public static class ViewsContainerHelper
    {
        public sealed class ViewManagerItem
        {
            required public Type TypeView;
            required public Type TypeViewModel;
        }

        public static ViewManagerItem New<TViewModel, TView>()
                        where TViewModel : class, ICrossViewModel
                        where TView : ICrossView<TViewModel>
        {
            return new ViewManagerItem
            {
                TypeView = typeof(TView),
                TypeViewModel = typeof(TViewModel),
            };
        }

        public static void RegisterViewModels(ViewManagerItem[] items)
        {
            var viewsContainers = Singleton<ViewsContainers>.Instance;
            foreach (var item in items)
            {
                viewsContainers.ViewViewModels.Add(item.TypeView, item.TypeViewModel);
                viewsContainers.ViewModelViews.Add(item.TypeViewModel, item.TypeView);
            }
        }
    }
}
