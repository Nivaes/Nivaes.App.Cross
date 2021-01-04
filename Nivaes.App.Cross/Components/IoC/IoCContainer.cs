// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{
    using System;
    using Autofac;
    using MvvmCross.Exceptions;

    public static class IoCContainer
    {
        private static Lazy<IComponentContext> mComponentContext = new Lazy<IComponentContext>(CreateComponentContext);

        private static ContainerBuilder? mContainerBuilder;

        public static IComponentContext ComponentContext => mComponentContext.Value;

        public static void RegisterTypes(Action<ContainerBuilder> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            mContainerBuilder = new ContainerBuilder();
            action.Invoke(mContainerBuilder);
            var context = mContainerBuilder.Build();

            mComponentContext = new Lazy<IComponentContext>(context);
        }

        private static IComponentContext CreateComponentContext()
        {
            throw new MvxException("You must call 'RegisterTypes' before calling 'ComponentContext'.");
        }
    }
}
