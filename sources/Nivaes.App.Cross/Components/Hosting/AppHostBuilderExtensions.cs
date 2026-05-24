using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Nivaes.App.Cross.Hosting
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseCrossApp<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this CrossAppBuilder builder)
        where TApp : class, IApplication
        {
            //builder.UseMauiPrimaryApp<TApp>();
            //builder.SetupXamlDefaults();
            return builder;
        }

        public static CrossAppBuilder UseCrossApp<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this CrossAppBuilder builder, Func<IServiceProvider, TApp> implementationFactory)
        where TApp : class, IApplication
        {
            //builder.UseMauiPrimaryApp<TApp>(implementationFactory);
            //builder.SetupXamlDefaults();
            return builder;
        }
    }
}
