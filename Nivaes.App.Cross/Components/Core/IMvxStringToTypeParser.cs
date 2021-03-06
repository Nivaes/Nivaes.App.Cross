﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;

    public interface IMvxStringToTypeParser
    {
        bool TypeSupported(Type targetType);

        object ReadValue(string? rawValue, Type targetType, string? fieldOrParameterName);
    }
}
