// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public class MvxWrappedFrame
        : IMvxWindowsFrame
    {
        private readonly Frame mFrame;

        public MvxWrappedFrame(Frame frame)
        {
            mFrame = frame;
        }

        public Control UnderlyingControl => mFrame;

        public object Content => mFrame.Content;

        public bool CanGoBack => mFrame.CanGoBack;

        public bool Navigate(Type viewType, object parameter)
        {
            return mFrame.Navigate(viewType, parameter);
        }

        public void GoBack()
        {
            mFrame.GoBack();
        }

        public void ClearValue(DependencyProperty property)
        {
            mFrame.ClearValue(property);
        }

        public object GetValue(DependencyProperty property)
        {
            return mFrame.GetValue(property);
        }

        public void SetValue(DependencyProperty property, object value)
        {
            mFrame.SetValue(property, value);
        }

        public void SetNavigationState(string state)
        {
            mFrame.SetNavigationState(state);
        }

        public string GetNavigationState()
        {
            return mFrame.GetNavigationState();
        }
    }
}
