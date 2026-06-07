using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Droid
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseDroidApp(this CrossAppBuilder builder/*, CrossApplication app*/)
        {
            builder.SetupDefaults(/*app*/);
            
            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder/*, CrossApplication app*/ )
        {
            builder.Services.TryAddSingleton<ICrossViewDispatcher, MvxAndroidViewDispatcher>();

            //builder.Services.TryAddSingleton<ICrossWindowsFrame>(sp => new CrossWindowsFrame(app.RootFrame!));
            //builder.Services.TryAddSingleton<IMvxWindowsViewPresenter, MvxMultiWindowViewPresenter>();
            //builder.Services.TryAddSingleton<ICrossViewsContainer, CrossWindowsViewsContainer>();

            //builder.Services.TryAddSingleton<ICrossWindowsViewModelRequestTranslator, CrossWindowsViewsContainer>();

            return builder;
        } 
    }
}
