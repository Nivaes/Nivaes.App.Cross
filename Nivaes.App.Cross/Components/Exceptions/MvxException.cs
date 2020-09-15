// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Exceptions
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [Serializable]
    public class MvxException : Exception
    {
        public MvxException()
        {
        }

        public MvxException(string message)
            : base(message)
        {
        }

        public MvxException(string messageFormat, params object[] messageFormatArguments)
            : base(string.Format(CultureInfo.CurrentCulture, messageFormat, messageFormatArguments))
        {
        }

        // the order of parameters here is slightly different to that normally expected in an exception
        // - but this order allows us to put string.Format in place
        public MvxException(Exception innerException, string messageFormat, params object[] formatArguments)
            : base(string.Format(CultureInfo.CurrentCulture, messageFormat, formatArguments), innerException)
        {
        }

        public MvxException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MvxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
