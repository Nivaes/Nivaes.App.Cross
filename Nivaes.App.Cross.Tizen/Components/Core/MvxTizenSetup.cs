﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Core
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
    using MvvmCross.Platforms.Tizen.Binding;
    using MvvmCross.Platforms.Tizen.Presenters;
    using MvvmCross.Platforms.Tizen.Views;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;

    public abstract class MvxTizenSetup<TApplication>
        : MvxSetup, IMvxTizenSetup
            where TApplication : class, ICrossApplication, new()
    {
        protected CoreApplication CoreApplication { get; private set; }

        public virtual void PlatformInitialize(CoreApplication coreApplication)
        {
            CoreApplication = coreApplication;
        }

        private IMvxTizenViewPresenter _presenter;
        protected IMvxTizenViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreateViewPresenter();
                return _presenter;
            }
        }

        protected override Task InitializeFirstChance()
        {
            RegisterPresenter();
            return base.InitializeFirstChance();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateTizenViewsContainer();
            Mvx.IoCProvider.RegisterSingleton(container);
            return container;
        }

        protected virtual IMvxTizenViewsContainer CreateTizenViewsContainer()
        {
            return new MvxTizenViewsContainer();
        }

        protected virtual IMvxTizenViewPresenter CreateViewPresenter()
        {
            return new MvxTizenViewPresenter();
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.IoCProvider.RegisterSingleton(presenter);
            Mvx.IoCProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        protected override Task InitializeLastChance()
        {
            InitializeBindingBuilder();
            return base.InitializeLastChance();
        }

        protected virtual void InitializeBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxTizenBindingBuilder();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxTizenViewDispatcher(Presenter);
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View");
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
