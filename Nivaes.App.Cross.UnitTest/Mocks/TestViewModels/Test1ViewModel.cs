// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.UnitTest.Mocks.TestViewModels
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;

    public class Test1ViewModel
        : MvxViewModel
    {
        public ITestThing? Thing { get; private set; }
        public IMvxBundle? BundleInit { get; private set; }
        public IMvxBundle? BundleState { get; private set; }
        public bool StartCalled { get; private set; }
        public string? TheInitString1Set { get; private set; }
        public Guid? TheInitGuid1Set { get; private set; }
        public Guid? TheInitGuid2Set { get; private set; }
        public BundleObject TheInitBundleSet { get; private set; }
        public string TheReloadString1Set { get; private set; }
        public Guid? TheReloadGuid1Set { get; private set; }
        public Guid? TheReloadGuid2Set { get; private set; }
        public BundleObject? TheReloadBundleSet { get; private set; }

        public Test1ViewModel(ITestThing thing)
        {
            Thing = thing;
        }

        public void Init(string theString1)
        {
            TheInitString1Set = theString1;
        }

        public void Init(Guid theGuid1, Guid theGuid2)
        {
            TheInitGuid1Set = theGuid1;
            TheInitGuid2Set = theGuid2;
        }

        public void Init(BundleObject bundle)
        {
            TheInitBundleSet = bundle;
        }

        protected override ValueTask InitFromBundle(IMvxBundle parameters)
        {
            BundleInit = parameters;

            return InitFromBundle(parameters);
        }

        public void ReloadState(string TheString1)
        {
            TheReloadString1Set = TheString1;
        }

        public void ReloadState(Guid theGuid1, Guid theGuid2)
        {
            TheReloadGuid1Set = theGuid1;
            TheReloadGuid2Set = theGuid2;
        }

        public void ReloadState(BundleObject bundle)
        {
            TheReloadBundleSet = bundle;
        }

        protected override ValueTask ReloadFromBundle(IMvxBundle state)
        {
            BundleState = state;

            return base.ReloadFromBundle(state);
        }

        public override ValueTask Start()
        {
            StartCalled = true;

            return base.Start();
        }
    }
}
