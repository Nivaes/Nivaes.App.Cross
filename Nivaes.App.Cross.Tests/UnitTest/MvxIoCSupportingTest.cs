// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Core;
using MvvmCross.IoC;
using Nivaes.App.Cross;
using Nivaes.App.Cross.IoC;
using Nivaes.App.Cross.Logging;

namespace MvvmCross.Tests
{
    [Obsolete("Eliminar")]
    public class MvxIoCSupportingTest
    {
        private TestLogger _logger;

        public IIoCProvider Ioc { get; private set; }

        public void Setup()
        {
            ClearAll();
        }

        public void Reset()
        {
            MvxSingleton.ClearAllSingletons();
            Ioc = null;
        }

        
        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return null;
        }

        [Obsolete("Usar AppCenter")]
        public virtual void ClearAll(IMvxIocOptions options = null)
        {
            // fake set up of the IoC
            Reset();
            var logProvider = CreateLogProvider();
            var log = CreateLog(logProvider);
            Ioc = IoCProvider.Initialize(options ?? CreateIocOptions());
            Ioc.RegisterSingleton(Ioc);
            Ioc.RegisterSingleton(logProvider);
            Ioc.RegisterSingleton(log);

            InitializeSingletonCache();
            InitializeMvxSettings();
            AdditionalSetup();
        }

        [Obsolete("Usar AppCenter")]
        public void InitializeSingletonCache()
        {
            if (MvxSingletonCache.Instance == null)
                MvxSingletonCache.Initialize();

            if (MvxBindingSingletonCache.Instance == null)
                MvxBindingSingletonCache.Initialize();
        }

        protected virtual void InitializeMvxSettings()
        {
            Ioc.RegisterSingleton<IMvxSettings>(new MvxSettings());
        }

        protected virtual void AdditionalSetup()
        {
        }

        [Obsolete("Usar AppCenter")]
        public void SetupTestLogger(TestLogger logger)
        {
            _logger = logger;

            var logProvider = CreateLogProvider();
            var log = CreateLog(logProvider);

            Ioc.RegisterSingleton(logProvider);
            Ioc.RegisterSingleton(log);
        }

        [Obsolete("Usar AppCenter", true)]
        protected virtual IMvxLogProvider CreateLogProvider()
        {
            var logProvider = new TestLogProvider(_logger);
            return logProvider;
        }

        [Obsolete("Usar AppCenter", true)]
        protected virtual IMvxLog CreateLog(IMvxLogProvider logProvider)
        {
            var globalLog = logProvider.GetLogFor<MvxIoCSupportingTest>();
            MvxLog.Instance = globalLog;

            return globalLog;
        }

        public void SetInvariantCulture()
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;
        }
    }
}
