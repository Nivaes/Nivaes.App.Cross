namespace Nivaes.App.Cross
{
    public static class ViewsStoreManager
    {
        public sealed class ViewManagerItem
        {
            internal NameViewsStore.KeyStoreItem NameViews { get; set; }
            internal NameViewModelsStore.KeyStoreItem NameViewModels { get; set; }
            internal ViewModelViewsStore.KeyStoreItem TypeViewModels { get; set; }
            internal ViewsViewModelStore.KeyStoreItem TypeViews { get; set; }
        }

        public static ViewManagerItem New<TViewModel, TView>()
                        where TViewModel : class, ICrossViewModel
                        where TView : ICrossView<TViewModel>
        {
            return new ViewManagerItem()
            {
                NameViews = new NameViewsStore.KeyStoreItem { Key = typeof(TView).FullName!.GetHashCode(), Value = typeof(TView) },
                NameViewModels = new NameViewModelsStore.KeyStoreItem { Key = typeof(TViewModel).FullName!.GetHashCode(), Value = typeof(TViewModel) },
                TypeViews = new ViewsViewModelStore.KeyStoreItem { Key = typeof(TView).GetHashCode(), Value = typeof(TViewModel) },
                TypeViewModels = new ViewModelViewsStore.KeyStoreItem { Key = typeof(TViewModel).GetHashCode(), Value = typeof(TView) },
            };
        }

        public static void RegisterViewModels(ViewManagerItem[] items)
        {
            Singleton<NameViewsStore>.Instance.Merge(items.Select(x => x.NameViews).ToArray());
            Singleton<NameViewModelsStore>.Instance.Merge(items.Select(x => x.NameViewModels).ToArray());
            Singleton<ViewModelViewsStore>.Instance.Merge(items.Select(x => x.TypeViewModels).ToArray());
            Singleton<ViewsViewModelStore>.Instance.Merge(items.Select(x => x.TypeViews).ToArray());
        }
    }
}
