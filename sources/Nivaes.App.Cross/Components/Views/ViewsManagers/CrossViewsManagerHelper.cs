namespace Nivaes.App.Cross
{
    public static class CrossViewsManagerHelper
    {
        public sealed class ViewManagerItem
        {
            internal CrossNameViewsManager.KeyStoreItem NameStoreItem { get; set; }
            internal CrossViewModelViewsManager.KeyStoreItem TypeModel { get; set; }
            internal CrossViewsViewModelManager.KeyStoreItem TypeViewModel { get; set; }
        }

        public static ViewManagerItem New<TViewModel, TView>()
                        where TViewModel : class, ICrossViewModel
                        where TView : ICrossView<TViewModel>
        {
            return new ViewManagerItem()
            {
                NameStoreItem = new CrossNameViewsManager.KeyStoreItem { Key = typeof(TViewModel).FullName!.GetHashCode(), Value = typeof(TView) },
                TypeModel = new CrossViewModelViewsManager.KeyStoreItem { Key = typeof(TViewModel).GetHashCode(), Value = typeof(TView) },
                TypeViewModel = new CrossViewsViewModelManager.KeyStoreItem { Key = typeof(TView).GetHashCode(), Value = typeof(TViewModel) } 
            };
        }

        public static void RegisterViewModel(ViewManagerItem[] items) 
        {
            var viewModelViewsManager = new CrossViewModelViewsManager(items.Select(x => x.TypeModel).ToArray());
            var nameViewsManager = new CrossNameViewsManager(items.Select(x => x.NameStoreItem).ToArray());
            var ViewsManager = new CrossViewsViewModelManager(items.Select(x => x.TypeViewModel).ToArray());

            Singleton<CrossViewModelViewsManager>.Add(viewModelViewsManager);
            Singleton<CrossNameViewsManager>.Add(nameViewsManager);
            Singleton<CrossViewsViewModelManager>.Add(ViewsManager);
        }
    }
}
