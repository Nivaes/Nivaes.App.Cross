namespace Nivaes.App.Cross
{
    public static class ViewsContainerManagerHelper
    {
        public sealed class ViewManagerItem
        {
            internal NameViewsKeyContainerManager.KeyStoreItem NameViews { get; set; }
            internal NameViewModelsKeyContainerManager.KeyStoreItem NameViewModels { get; set; }
            internal ViewModelViewsKeyContainerManager.KeyStoreItem TypeViewModels { get; set; }
            internal ViewsViewKeyContainerManager.KeyStoreItem TypeViews { get; set; }
        }

        public static ViewManagerItem New<TViewModel, TView>()
                        where TViewModel : class, ICrossViewModel
                        where TView : ICrossView<TViewModel>
        {
            return new ViewManagerItem()
            {
                NameViews = new NameViewsKeyContainerManager.KeyStoreItem { Key = typeof(TView).FullName!.GetHashCode(), Value = typeof(TView) },
                NameViewModels = new NameViewModelsKeyContainerManager.KeyStoreItem { Key = typeof(TViewModel).FullName!.GetHashCode(), Value = typeof(TViewModel) },
                TypeViews = new ViewsViewKeyContainerManager.KeyStoreItem { Key = typeof(TView).GetHashCode(), Value = typeof(TViewModel) },
                TypeViewModels = new ViewModelViewsKeyContainerManager.KeyStoreItem { Key = typeof(TViewModel).GetHashCode(), Value = typeof(TView) },
            };
        }

        public static void RegisterViewModels(ViewManagerItem[] items)
        {
            Singleton<NameViewsKeyContainerManager>.Instance.Merge(items.Select(x => x.NameViews).ToArray());
            Singleton<NameViewModelsKeyContainerManager>.Instance.Merge(items.Select(x => x.NameViewModels).ToArray());
            Singleton<ViewModelViewsKeyContainerManager>.Instance.Merge(items.Select(x => x.TypeViewModels).ToArray());
            Singleton<ViewsViewKeyContainerManager>.Instance.Merge(items.Select(x => x.TypeViews).ToArray());
        }
    }
}
