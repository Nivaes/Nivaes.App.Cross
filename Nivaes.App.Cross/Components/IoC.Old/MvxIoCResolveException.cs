// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Obsolete("Sustituir por Autofac")]
    [Serializable]
    public class MvxIoCResolveException
        : MvxException
    {
        public MvxIoCResolveException()
        {
        }

        public MvxIoCResolveException(string message)
            : base(message)
        {
        }

        public MvxIoCResolveException(string messageFormat, params object[] messageFormatArguments)
            : base(messageFormat, messageFormatArguments)
        {
        }

        public MvxIoCResolveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MvxIoCResolveException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
