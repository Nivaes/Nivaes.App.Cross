﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using MvvmCross.IoC;
    using MvvmCross.Plugin;
    using Nivaes.App.Cross;

    public abstract class CrossApplication
        : ICrossApplication
    {
        [Obsolete("Quitar la sobrecarga de localizadores. Todo por Roslyn.", true)]
        private IMvxViewModelLocator? mDefaultLocator;

        [Obsolete("Quitar la sobrecarga de localizadores. Todo por Roslyn.", true)]
        private IMvxViewModelLocator DefaultLocator
        {
            get
            {
                mDefaultLocator ??= CreateDefaultViewModelLocator();
                return mDefaultLocator;
            }
        }

        protected virtual IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            return new MvxDefaultViewModelLocator();
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
            // do nothing
        }

        /// <summary>
        /// Any initialization steps that can be done in the background
        /// </summary>
        public virtual ValueTask Initialize()
        {
            return new ValueTask();
        }

        /// <summary>
        /// Any initialization steps that need to be done on the UI thread
        /// </summary>
        public virtual ValueTask Startup()
        {
            return new ValueTask();
        }

        /// <summary>
        /// If the application is restarted (eg primary activity on Android 
        /// can be restarted) this method will be called before Startup
        /// is called again
        /// </summary>
        public virtual void Reset()
        {
            // do nothing
        }

        [Obsolete("Quitar la sobrecarga de localizadores. Todo por Roslyn.")]
        public IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            return DefaultLocator;
        }

        protected void RegisterCustomAppStart<TMvxAppStart>()
            where TMvxAppStart : class, IMvxAppStart
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, TMvxAppStart>();
        }

        protected void RegisterAppStart<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel>>();
        }

        protected void RegisterAppStart(IMvxAppStart appStart)
        {
            Mvx.IoCProvider.RegisterSingleton(appStart);
        }

        protected virtual void RegisterAppStart<TViewModel, TParameter>()
          where TViewModel : IMvxViewModel<TParameter>
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel, TParameter>>();
        }

        protected IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        protected IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }
    }

    public class CrossApplication<TParameter>
        : CrossApplication, ICrossApplication<TParameter>
    {
        public virtual TParameter Startup(TParameter parameter)
        {
            // do nothing, so just return the original hint
            return parameter;
        }
    }
}
