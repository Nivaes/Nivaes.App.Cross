namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Presenters;

    public class ViewsManager
    {
        private IDictionary<int, Type> mViewModelViews { get; } = new Dictionary<int, Type>();

        public ViewsManager()
        {
        }

        public void AddViewModelView<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : IView
        {
            mViewModelViews.Add(typeof(TViewModel).GetHashCode(), typeof(TView));
        }

        public bool TryGetValue<TViewModel>([MaybeNullWhen(false)]  out Type viewType)
            where TViewModel : IViewModel
        {
            return mViewModelViews.TryGetValue(typeof(TViewModel).GetHashCode(), out viewType);
        }
    }
}
