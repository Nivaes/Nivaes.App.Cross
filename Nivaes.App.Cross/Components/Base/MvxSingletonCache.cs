// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
    using System;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using MvvmCross.ViewModels;

    [Obsolete("Eliminar")]
    public sealed class MvxSingletonCache
        : MvxSingleton<IMvxSingletonCache>, IMvxSingletonCache
    {
        public static MvxSingletonCache Initialize()
        {
            if (Instance != null)
                throw new MvxException("You should only initialize MvxBindingSingletonCache once");

            var instance = new MvxSingletonCache();
            return instance;
        }

        private MvxSingletonCache()
        {
        }

        private bool mInpcInterceptorResolveAttempted;
        private IMvxInpcInterceptor? mInpcInterceptor;

        public IMvxInpcInterceptor InpcInterceptor
        {
            get
            {
                if (mInpcInterceptorResolveAttempted)
                    return mInpcInterceptor;

                Mvx.IoCProvider.TryResolve<IMvxInpcInterceptor>(out mInpcInterceptor);
                mInpcInterceptorResolveAttempted = true;
                return mInpcInterceptor;
            }
        }

        private IMvxStringToTypeParser? mParser;

        public IMvxStringToTypeParser Parser
        {
            get
            {
                mParser ??= Mvx.IoCProvider.Resolve<IMvxStringToTypeParser>();
                return mParser;
            }
        }

        private IMvxSettings? mSettings;

        public IMvxSettings Settings
        {
            get
            {
                mSettings ??= Mvx.IoCProvider.Resolve<IMvxSettings>();
                return mSettings;
            }
        }
    }
}
