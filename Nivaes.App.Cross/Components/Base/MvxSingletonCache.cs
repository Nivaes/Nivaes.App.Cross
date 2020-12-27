// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
    using System;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;

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
