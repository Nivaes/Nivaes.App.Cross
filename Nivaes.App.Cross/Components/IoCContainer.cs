// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;
    using Autofac;

    public static class IoCContainer
    {
        private static readonly Lazy<IContainer> mContainer = new Lazy<IContainer>(CreateContainer);

        public static IContainer Container => mContainer.Value;

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsoleOutput>().As<IOutput>();
            //builder.RegisterType<TodayWriter>().As<IDateWriter>();

            var container = builder.Build();
            return container;
        }
    }
}
