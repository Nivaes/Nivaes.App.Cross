﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Autofac;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Converters;
    using MvvmCross.Core;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Ios;
    using MvvmCross.Platforms.Ios.Binding;
    using MvvmCross.Platforms.Ios.Views;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;
    using UIKit;

    public class MvxIosSetup<TApplication>
        : MvxSetup, IMvxIosSetup
            where TApplication : class, ICrossApplication, new()
    {
        protected IMvxApplicationDelegate? ApplicationDelegate { get; private set; }
        protected UIWindow? Window { get; private set; }

        private IMvxIosViewPresenter? mPresenter;

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            Window = window;
            ApplicationDelegate = applicationDelegate;
        }

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
        {
            ApplicationDelegate = applicationDelegate;
            mPresenter = presenter;
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateIosViewsContainer();
            RegisterIosViewCreator(container);
            return container;
        }

        protected virtual IMvxIosViewsContainer CreateIosViewsContainer()
        {
            return new MvxIosViewsContainer();
        }

        protected virtual void RegisterIosViewCreator(IMvxIosViewsContainer container)
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxIosViewCreator>(container);
            Mvx.IoCProvider.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxIosViewDispatcher(Presenter);
        }

        protected override async Task InitializeFirstChance()
        {
            await Task.Run(() =>
            {
                RegisterPlatformProperties();
                RegisterPresenter();
                RegisterLifetime();
            }).ConfigureAwait(false);

            await base.InitializeFirstChance().ConfigureAwait(false);
        }

        protected virtual void RegisterPlatformProperties()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxIosSystem>(CreateIosSystemProperties());
        }

        protected virtual MvxIosSystem CreateIosSystemProperties()
        {
            return new MvxIosSystem();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(ApplicationDelegate);
        }

        protected IMvxIosViewPresenter Presenter
        {
            get
            {
                mPresenter ??= CreateViewPresenter();
                return mPresenter;
            }
        }

        protected virtual IMvxIosViewPresenter CreateViewPresenter()
        {
            return new MvxIosViewPresenter(ApplicationDelegate, Window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.IoCProvider.RegisterSingleton(presenter);
            Mvx.IoCProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        protected override async Task InitializeLastChance()
        {
            await InitializeBindingBuilder().ConfigureAwait(false);
            await base.InitializeLastChance().ConfigureAwait(false);
        }

        protected virtual Task InitializeBindingBuilder()
        {
            return Task.Run(() =>
            {
                RegisterBindingBuilderCallbacks();
                var bindingBuilder = CreateBindingBuilder();
                bindingBuilder.DoRegistration();
            });
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxIosBindingBuilder();
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        [Obsolete("Not user reflector")]
        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }

        protected override ICrossApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();

        [Obsolete("Not user reflector")]
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }

        protected override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
