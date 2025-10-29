//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Nivaes.App.Cross.Presenters
//{
//    public class ViewPresenterManager
//    {
//        private IDictionary<Type, IPresenterAction> ViewPresenters { get; } = new Dictionary<Type, IPresenterAction>();

//        public ViewPresenterManager()
//        {
//        }

//        public void AddViewPresenter<TViewModel>(IPresenterAction pressenterAction)
//            where TViewModel : IViewModel
//        {
//            ViewPresenters.Add(typeof(TViewModel), pressenterAction);
//        }

//        public IPresenterAction? GetPressenter(Type viewType)
//        {
//            if (ViewPresenters.TryGetValue(viewType, out IPresenterAction? pressenterAction))
//            {
//                return pressenterAction;
//            }
//            return null;
//        }

//    }
//}
