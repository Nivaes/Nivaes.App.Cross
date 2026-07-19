using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class SplitViewPressenterAction 
        : PressenterAction<SplitViewPresentationAttribute>
    {
        #region Constructor
        public SplitViewPressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ICrossWindowsViewModelRequestTranslator requestTranslator,
                ILogger<SplitViewPressenterAction> logger)
            : base(context, rootFrame, requestTranslator,  logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, SplitViewPresentationAttribute attribute, ViewModelRequest request)
        {
            var windowInformation = GetWindowInformation(request);
            if (windowInformation.RootFrame.Content is ICrossWindowsView currentPage)
            {
                var splitView = currentPage.Content.FindControl<SplitView>();
                if (splitView == null)
                {
                    return ValueTask.FromResult(false);
                }

                if (attribute.Position == SplitPanePosition.Content)
                {
                    var nestedFrame = splitView.Content as Frame;
                    if (nestedFrame == null)
                    {
                        nestedFrame = new Frame();
                        splitView.Content = nestedFrame;
                    }

                    var requestText = GetRequestText(request);
                    nestedFrame.Navigate(viewType, requestText);

                    //if (request is CrossViewModelInstanceRequest instanceReq && instanceReq.ViewModelInstance != null)
                    //{
                    //    windowInformation.RegisterSubViewModel(instanceReq.ViewModelInstance);
                    //}
                    windowInformation.RegisterSubViewModel(request.ViewModel);
                }
                else if (attribute.Position == SplitPanePosition.Pane)
                {
                    var nestedFrame = splitView.Pane as Frame;
                    if (nestedFrame == null)
                    {
                        nestedFrame = new Frame();
                        splitView.Pane = nestedFrame;
                    }

                    var requestText = GetRequestText(request);
                    nestedFrame.Navigate(viewType, requestText);

                    //if (request is CrossViewModelInstanceRequest instanceReq && instanceReq.ViewModelInstance != null)
                    //{
                    //    windowInformation.RegisterSubViewModel(instanceReq.ViewModelInstance);
                    //}
                    windowInformation.RegisterSubViewModel(request.ViewModel);
                }
            }

            return ValueTask.FromResult(true);
        }       

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, SplitViewPresentationAttribute attribute)
        {
            return ClosePage(viewModel, attribute);
        }


    }
}
