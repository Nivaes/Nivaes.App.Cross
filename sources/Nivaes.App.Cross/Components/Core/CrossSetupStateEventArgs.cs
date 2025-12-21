namespace Nivaes.App.Cross
{
    using System;
    using MvvmCross.Core;

    public class CrossSetupStateEventArgs 
        : EventArgs
    {
        public CrossSetupStateEventArgs(CrossSetupState setupState)
        {
            SetupState = setupState;
        }

        public CrossSetupState SetupState { get; }
    }
}
