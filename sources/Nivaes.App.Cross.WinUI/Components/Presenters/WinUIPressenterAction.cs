//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Nivaes.App.Cross.Presenters;

//namespace Nivaes.App.Cross.WinUI.Presenters
//{
//    public class WinUIPressenterAction<TView> : PresenterAction<TView>, IPresenterAction
//        where TView : IView
//    {
//        public WinUIPressenterAction()
//        {
//        }

//        public Task ShowPage()
//        {
//            var viewType = Singleton<ViewPresenterManager>.Instance.GetPressenter(base.ViewType);

//            if(viewType != null)
//            {
//                //WrappedFrame.Navigate(viewType.GetType(), null);
//            }
            

//            return Task.CompletedTask;
//        }
//    }
//}
