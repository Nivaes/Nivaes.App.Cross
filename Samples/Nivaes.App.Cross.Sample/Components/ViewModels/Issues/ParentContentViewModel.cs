// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using MvvmCross;
    using MvvmCross.Commands;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class ParentContentViewModel
        : MvxViewModel
    {
        private ChildContentViewModel? mChildViewModel1;

        public ChildContentViewModel? ChildViewModel1
        {
            get => mChildViewModel1;
            set
            {
                SetProperty(ref mChildViewModel1, value);
            }
        }


        private ChildContentViewModel? mChildBindingContext2;
        public ChildContentViewModel? ChildBindingContext2
        {
            get => mChildBindingContext2;
            set
            {
                SetProperty(ref mChildBindingContext2, value);
            }
        }

        private bool mChildViewModelEnabled;

        public bool ChildViewModelEnabled
        {
            get => mChildViewModelEnabled;
            set
            {
                SetProperty(ref mChildViewModelEnabled, value);
            }
        }

        public IMvxCommand ChangeButtonCmd1 => new MvxCommand(() => ChildViewModel1!.Test = (ChildViewModel1.Test == "Bound Text 1" ? "Bound Text 2" : "Bound Text 1"));
        public IMvxCommand ToggleChild1EnabledCmd => new MvxCommand(() => ChildViewModelEnabled = !ChildViewModelEnabled);

        public IMvxCommand ChangeButtonCmd2 => new MvxCommand(() => ChildBindingContext2!.Test = (ChildBindingContext2.Test == "Bound Text 1" ? "Bound Text 2" : "Bound Text 1"));

        public override ValueTask Prepare()
        {
            var vm = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<ChildContentViewModel>.GetDefaultRequest(), null) as ChildContentViewModel;
            vm.Test = "Child 1";
            ChildViewModel1 = vm;
            var bc = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>()
                    .LoadViewModel(MvxViewModelRequest<ChildContentViewModel>.GetDefaultRequest(), null) as ChildContentViewModel;

            bc.Test = "Child 2";
            ChildBindingContext2 = bc;

            return base.Prepare();
        }
    }
}
