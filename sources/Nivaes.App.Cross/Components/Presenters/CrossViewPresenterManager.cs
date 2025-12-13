namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;

    public class CrossViewPresenterManager
    {
        private IDictionary<Type, ICrossPresenterAction> ViewPresenters { get; } = new Dictionary<Type, ICrossPresenterAction>();

        public CrossViewPresenterManager()
        {
        }

        public void AddViewPresenter<TViewModel>(ICrossPresenterAction pressenterAction)
            where TViewModel : ICrossViewModel
        {
            ViewPresenters.Add(typeof(TViewModel), pressenterAction);
        }

        public ICrossPresenterAction? GetPressenter(Type viewType)
        {
            if (ViewPresenters.TryGetValue(viewType, out ICrossPresenterAction? pressenterAction))
            {
                return pressenterAction;
            }
            return null;
        }

    }
}
