﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.macOS
{

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using AppKit;
    using Autofac;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Converters;
    using MvvmCross.Core;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Mac.Binding;
    using MvvmCross.Platforms.Mac.Presenters;
    using MvvmCross.Platforms.Mac.Views;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;

    public class MvxMacSetup<TApplication>
        : MvxSetup, IMvxMacSetup
            where TApplication : class, ICrossApplication, new()
    {
        private IMvxApplicationDelegate _applicationDelegate;
        private NSWindow _window;

        private IMvxMacViewPresenter _presenter;

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, NSWindow window)
        {
            PlatformInitialize(applicationDelegate);
            _window = window;
        }

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            PlatformInitialize(applicationDelegate);
            _presenter = presenter;
        }

        protected NSWindow Window
        {
            get { return _window; }
        }

        protected IMvxApplicationDelegate ApplicationDelegate
        {
            get { return _applicationDelegate; }
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateMacViewsContainer();
            RegisterMacViewCreator(container);
            return container;
        }

        protected virtual IMvxMacViewsContainer CreateMacViewsContainer()
        {
            return new MvxMacViewsContainer();
        }

        protected void RegisterMacViewCreator(IMvxMacViewsContainer container)
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxMacViewCreator>(container);
            Mvx.IoCProvider.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxMacViewDispatcher(_presenter);
        }

        protected override Task InitializeFirstChance()
        {
            RegisterPresenter();
            RegisterLifetime();

            return base.InitializeFirstChance();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxMacViewPresenter Presenter
        {
            get
            {
                _presenter ??= CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxMacViewPresenter CreateViewPresenter()
        {
            return new MvxMacViewPresenter(_applicationDelegate);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = this.Presenter;
            Mvx.IoCProvider.RegisterSingleton(presenter);
            Mvx.IoCProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        protected override Task InitializeLastChance()
        {
            InitialiseBindingBuilder();

            return base.InitializeLastChance();
        }

        protected virtual void InitialiseBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(this.FillValueConverters);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(this.FillTargetFactories);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(this.FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            var bindingBuilder = new MvxMacBindingBuilder();
            return bindingBuilder;
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        [Obsolete("Not user reflector")]
        protected virtual List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual IEnumerable<Type> ValueConverterHolders
        {
            get { return null; }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
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
