// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.IoC
{
    [Obsolete("Sustituir por Autofac")]
    public class MvxPropertyInjectorOptions
        : IMvxPropertyInjectorOptions
    {
        public MvxPropertyInjectorOptions()
        {
            InjectIntoProperties = MvxPropertyInjection.None;
            ThrowIfPropertyInjectionFails = false;
        }

        public MvxPropertyInjection InjectIntoProperties { get; set; }
        public bool ThrowIfPropertyInjectionFails { get; set; }

        private static IMvxPropertyInjectorOptions? mInjectProperties;

        public static IMvxPropertyInjectorOptions MvxInject
        {
            get
            {
                mInjectProperties ??= new MvxPropertyInjectorOptions()
                {
                    InjectIntoProperties = MvxPropertyInjection.MvxInjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return mInjectProperties;
            }
        }

        private static IMvxPropertyInjectorOptions? mAllProperties;

        public static IMvxPropertyInjectorOptions All
        {
            get
            {
                mAllProperties ??= new MvxPropertyInjectorOptions()
                {
                    InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return mAllProperties;
            }
        }
    }
}
