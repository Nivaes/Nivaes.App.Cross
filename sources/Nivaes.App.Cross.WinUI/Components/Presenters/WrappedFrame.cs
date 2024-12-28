//namespace Nivaes.App.Cross.WinUI.Presenters
//{
//    using System;
//    using Microsoft.UI.Xaml;
//    using Microsoft.UI.Xaml.Controls;

//    internal class WrappedFrame
//    {
//        private readonly Frame mFrame;

//        public WrappedFrame(Frame frame)
//        {
//            mFrame = frame;
//        }

//        public Control UnderlyingControl => mFrame;

//        public object Content => mFrame.Content;

//        public bool CanGoBack => mFrame.CanGoBack;

//        public bool Navigate(Type viewType, object parameter)
//        {
//            return mFrame.Navigate(viewType, parameter);
//        }

//        public void GoBack()
//        {
//            mFrame.GoBack();
//        }

//        public void ClearValue(DependencyProperty property)
//        {
//            mFrame.ClearValue(property);
//        }

//        public object GetValue(DependencyProperty property)
//        {
//            return mFrame.GetValue(property);
//        }

//        public void SetValue(DependencyProperty property, object value)
//        {
//            mFrame.SetValue(property, value);
//        }

//        public void SetNavigationState(string state)
//        {
//            mFrame.SetNavigationState(state);
//        }

//        public string GetNavigationState()
//        {
//            return mFrame.GetNavigationState();
//        }
//    }
//}
