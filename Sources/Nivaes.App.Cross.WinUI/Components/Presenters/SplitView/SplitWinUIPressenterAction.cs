using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class SplitWinUIPressenterAction 
        : WinUIPressenterAction<SplitViewPresentationAttribute>
    {
        #region Constructor
        public SplitWinUIPressenterAction(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                ICrossWindowsFrame rootFrame,
                ICrossWindowsViewModelRequestTranslator requestTranslator,
                ILogger<SplitWinUIPressenterAction> logger)
            : base(context, viewsContainer, rootFrame, requestTranslator,  logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, SplitViewPresentationAttribute attribute, CrossViewModelRequest request)
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

                    if (request is CrossViewModelInstanceRequest instanceReq && instanceReq.ViewModelInstance != null)
                    {
                        windowInformation.RegisterSubViewModel(instanceReq.ViewModelInstance);
                    }
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

                    if (request is CrossViewModelInstanceRequest instanceReq && instanceReq.ViewModelInstance != null)
                    {
                        windowInformation.RegisterSubViewModel(instanceReq.ViewModelInstance);
                    }
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
