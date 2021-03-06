﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.tvOS
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
    using MvvmCross.Platforms.Tvos;
    using MvvmCross.Platforms.Tvos.Binding;
    using MvvmCross.Platforms.Tvos.Presenters;
    using MvvmCross.Platforms.Tvos.Views;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;
    using UIKit;

    public class MvxTvosSetup<TApplication>
        : MvxSetup, IMvxTvosSetup
            where TApplication : class, ICrossApplication, new()
    {
        private IMvxApplicationDelegate _applicationDelegate;
        private UIWindow _window;

        private IMvxTvosViewPresenter _presenter;

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter)
        {
            _presenter = presenter;
            _applicationDelegate = applicationDelegate;
        }

        protected UIWindow Window => _window;

        protected IMvxApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateTvosViewsContainer();
            RegisterTvosViewCreator(container);
            return container;
        }

        protected virtual IMvxTvosViewsContainer CreateTvosViewsContainer()
        {
            return new MvxTvosViewsContainer();
        }

        protected virtual void RegisterTvosViewCreator(IMvxTvosViewsContainer container)
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxTvosViewCreator>(container);
            Mvx.IoCProvider.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxTvosViewDispatcher(Presenter);
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
            Mvx.IoCProvider.RegisterSingleton<IMvxTvosSystem>(CreateTvosSystemProperties());
        }

        protected virtual MvxTvosSystem CreateTvosSystemProperties()
        {
            return new MvxTvosSystem();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxTvosViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxTvosViewPresenter CreateViewPresenter()
        {
            return new MvxTvosViewPresenter(_applicationDelegate, _window);
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
            return new MvxTvosBindingBuilder();
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
