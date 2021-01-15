// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CrossException
        : Exception
    {
        public CrossException()
        {
        }

        public CrossException(string message)
            : base(message)
        {
        }

        public CrossException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CrossException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
