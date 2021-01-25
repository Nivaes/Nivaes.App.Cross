// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
    using System;
    using System.Reflection;
    using MvvmCross.Exceptions;
    using MvvmCross.IoC;
    using Nivaes.App.Cross.Logging;

    public class MvxBootstrapRunner
    {
        [Obsolete("Not user reflector")]
        public virtual void Run(Assembly assembly)
        {
            var types = assembly.CreatableTypes()
                                .Inherits<IMvxBootstrapAction>();

            foreach (var type in types)
            {
                Run(type);
            }
        }

        [Obsolete("Not user reflector")]
        protected virtual void Run(Type type)
        {
            //try
            //{
            var toRun = Activator.CreateInstance(type);
            if (toRun is not IMvxBootstrapAction bootstrapAction)
            {
                //MvxLog.Instance?.Warn("Could not run startup task {0} - it's not a startup task", type.Name);
                return;
            }

            bootstrapAction.Run();
            //}
            //catch (Exception exception)
            //{
            //    // pokemon handling
            //    MvxLog.Instance?.Warn("Error running startup task {0} - error {1}", type.Name, exception.ToLongString());
            //}
        }
    }
}
