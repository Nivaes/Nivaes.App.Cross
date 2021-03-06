﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.UnitTest
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public class SimpleParameterTestViewModel
        : MvxViewModel<string>
    {
        public string? Parameter { get; set; }

        public virtual void Init()
        {
        }

        public override ValueTask Prepare(string parameter)
        {
            Parameter = parameter;

            return new ValueTask();
        }
    }
}
