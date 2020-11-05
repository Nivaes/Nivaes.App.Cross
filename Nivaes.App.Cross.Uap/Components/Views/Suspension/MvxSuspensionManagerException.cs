// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views.Suspension
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using MvvmCross.Exceptions;

    [Serializable]
    [SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not message.")]
    public class MvxSuspensionManagerException
        : MvxException
    {
        public MvxSuspensionManagerException()
            : base("MvxSuspensionManager failed")
        {
        }

        public MvxSuspensionManagerException(Exception ex)
            : base("MvxSuspensionManager failed", ex)
        {
        }

        protected MvxSuspensionManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
