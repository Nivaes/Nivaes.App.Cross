﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel.Activation;

namespace Nivaes.App.Cross.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.UI.Xaml.Controls;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Converters;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Uap.Binding;
    using MvvmCross.Platforms.Uap.Presenters;
    using MvvmCross.Platforms.Uap.Views;
    using MvvmCross.Platforms.Uap.Views.Suspension;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;

    public abstract class MvxWindowsSetup<TApplication>
        : MvxSetup, IMvxWindowsSetup
            where TApplication : class, ICrossApplication, new()
    {
        private IMvxWindowsFrame? mRootFrame;
        private string? mSuspensionManagerSessionStateKey;
        private IMvxWindowsViewPresenter? mPresenter;

        public virtual void PlatformInitialize(Frame rootFrame, IActivatedEventArgs? activatedEventArgs,
            string? suspensionManagerSessionStateKey = null)
        {
            PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
            ActivationArguments = activatedEventArgs;
        }

        public virtual void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null)
        {
            PlatformInitialize(new MvxWrappedFrame(rootFrame));
            mSuspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
        }

        public virtual void PlatformInitialize(IMvxWindowsFrame rootFrame)
        {
            mRootFrame = rootFrame;
        }

        public virtual void UpdateActivationArguments(IActivatedEventArgs? e)
        {
            ActivationArguments = e;
        }

        protected override async Task InitializeFirstChance()
        {
            InitializeSuspensionManager();
            await RegisterPresenter().ConfigureAwait(false);
            await base.InitializeFirstChance().ConfigureAwait(false);
        }

        protected virtual void InitializeSuspensionManager()
        {
            var suspensionManager = CreateSuspensionManager();
            Mvx.IoCProvider.RegisterSingleton(suspensionManager);

            if (mSuspensionManagerSessionStateKey != null)
                suspensionManager.RegisterFrame(mRootFrame!, mSuspensionManagerSessionStateKey);
        }

        protected virtual IMvxSuspensionManager CreateSuspensionManager()
        {
            return new MvxSuspensionManager();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateStoreViewsContainer();
            Mvx.IoCProvider.RegisterSingleton<IMvxWindowsViewModelRequestTranslator>(container);
            Mvx.IoCProvider.RegisterSingleton<IMvxWindowsViewModelLoader>(container);

            if (!(container is MvxViewsContainer))
                throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");

            return container;
        }

        protected virtual IMvxStoreViewsContainer CreateStoreViewsContainer()
        {
            return new MvxWindowsViewsContainer();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return CreateViewDispatcher(mRootFrame!);
        }

        protected IMvxWindowsViewPresenter Presenter
        {
            get
            {
                mPresenter ??= CreateViewPresenter(mRootFrame!);
                return mPresenter;
            }
        }

        protected virtual IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            return new MvxWindowsViewPresenter(rootFrame);
        }

        protected virtual MvxWindowsViewDispatcher CreateViewDispatcher(IMvxWindowsFrame rootFrame)
        {
            return new MvxWindowsViewDispatcher(Presenter, rootFrame);
        }

        protected virtual Task RegisterPresenter()
        {
            var presenter = Presenter;

            return Task.Run(() =>
            {
                Mvx.IoCProvider.RegisterSingleton(presenter);
                Mvx.IoCProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
            });
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

        protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }

        protected IActivatedEventArgs? ActivationArguments { get; private set; }

        protected virtual List<Type> ValueConverterHolders => new();

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

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxWindowsBindingBuilder();
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }

        [Obsolete("Not user reflector")]
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }

        protected override ICrossApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();

        protected override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
