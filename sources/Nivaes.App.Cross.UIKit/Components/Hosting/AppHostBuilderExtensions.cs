using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.UIKitOS
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseUIKitApp(this CrossAppBuilder builder, UIWindow windows)
        {
            builder.SetupDefaults(windows);

            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder, UIWindow windows)
        {
            builder.Services.TryAddSingleton<ICrossViewDispatcher, MvxIosViewDispatcher>();
            builder.Services.TryAddSingleton<IMvxIosViewPresenter>(sp =>
            {
                var viewsContainer = sp.GetRequiredService<ICrossViewsContainer>();

                return new MvxIosViewPresenter(windows, viewsContainer);
            });

            builder.Services.TryAddSingleton<ICrossViewsContainer, MvxIosViewsContainer>();

            return builder;
        }
    }
}
