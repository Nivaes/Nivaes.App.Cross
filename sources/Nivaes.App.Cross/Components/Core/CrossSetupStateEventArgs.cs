namespace Nivaes.App.Cross
{
    using System;

    public class CrossSetupStateEventArgs : EventArgs
    {
        public CrossSetupStateEventArgs(CrossSetupState setupState)
        {
            SetupState = setupState;
        }

        public CrossSetupState SetupState { get; }
    }
}
