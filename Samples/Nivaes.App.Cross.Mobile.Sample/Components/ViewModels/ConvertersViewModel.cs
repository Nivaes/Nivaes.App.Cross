﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using MvvmCross.ViewModels;

    public class ConvertersViewModel
        : MvxViewModel
    {
        public string UppercaseConverterTest => "this text was lowercase";

        public string LowercaseConverterTest => "THIS TEXT WAS UPPERCASE";
    }
}