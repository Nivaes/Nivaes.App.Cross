﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class MvxCommandAttribute
        : Attribute
    {
        public MvxCommandAttribute(string commandName, string? canExecutePropertyName = null)
        {
            CanExecutePropertyName = canExecutePropertyName;
            CommandName = commandName;
        }

        public string CommandName { get; set; }

        public string? CanExecutePropertyName { get; set; }
    }
}
