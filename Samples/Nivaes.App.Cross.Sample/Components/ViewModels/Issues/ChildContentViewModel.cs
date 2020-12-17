// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;

    public class ChildContentViewModel
        : MvxViewModel
    {
        public ChildContentViewModel()
        {
        }

        private string mTest = string.Empty;

        public string Test
        {
            get => mTest;
            set { SetProperty(ref mTest, value); }
        }

        public override async ValueTask Initialize()
        {
            //Test = "Bound Text";
            await Task.Yield();
        }
    }
}
