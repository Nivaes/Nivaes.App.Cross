namespace Nivaes.App.Cross
{
    public static class CrossViewsManagerHelper
    {
        public sealed class ViewManagerItem
        {
            internal CrossNameViewsManager.KeyStoreItem NameViews { get; set; }
            internal CrossNameViewModelsManager.KeyStoreItem NameViewModels { get; set; }
            internal CrossViewModelViewsManager.KeyStoreItem TypeViewModels { get; set; }
            internal CrossViewsViewModelManager.KeyStoreItem TypeViews { get; set; }
        }

        public static ViewManagerItem New<TViewModel, TView>()
                        where TViewModel : class, ICrossViewModel
                        where TView : ICrossView<TViewModel>
        {
            return new ViewManagerItem()
            {
                NameViews = new CrossNameViewsManager.KeyStoreItem { Key = typeof(TView).FullName!.GetHashCode(), Value = typeof(TView) },
                NameViewModels = new CrossNameViewModelsManager.KeyStoreItem { Key = typeof(TViewModel).FullName!.GetHashCode(), Value = typeof(TViewModel) },
                TypeViews = new CrossViewsViewModelManager.KeyStoreItem { Key = typeof(TView).GetHashCode(), Value = typeof(TViewModel) },
                TypeViewModels = new CrossViewModelViewsManager.KeyStoreItem { Key = typeof(TViewModel).GetHashCode(), Value = typeof(TView) },
            };
        }

        public static void RegisterViewModels(ViewManagerItem[] items) 
        {
            Singleton<CrossNameViewsManager>.Instance.Merge(items.Select(x => x.NameViews).ToArray());
            Singleton<CrossNameViewModelsManager>.Instance.Merge(items.Select(x => x.NameViewModels).ToArray());
            Singleton<CrossViewModelViewsManager>.Instance.Merge(items.Select(x => x.TypeViewModels).ToArray());
            Singleton<CrossViewsViewModelManager>.Instance.Merge(items.Select(x => x.TypeViews).ToArray());
        }
    }
}
