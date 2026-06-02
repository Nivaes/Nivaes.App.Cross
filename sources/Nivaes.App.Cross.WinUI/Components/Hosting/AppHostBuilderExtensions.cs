using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.UI.Xaml.Controls;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.WinUI
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseWinUIApp(this CrossAppBuilder builder, CrossWinUIApplication app)
        {
            builder.SetupDefaults(app);
            
            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder, CrossWinUIApplication app)
        {
            builder.Services.TryAddSingleton<ICrossViewDispatcher, CrossWindowsViewDispatcher>();

            builder.Services.TryAddSingleton<ICrossWindowsFrame>(sp => new CrossWrappedFrame(app.RootFrame!));
            builder.Services.TryAddSingleton<IMvxWindowsViewPresenter, MvxMultiWindowViewPresenter>();
            builder.Services.TryAddSingleton<ICrossViewsContainer, CrossWindowsViewsContainer>();

            return builder;
        }
    }
}
