namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("Quitar IoC de Cross")]
    public class CrossSetupStateEventArgs : EventArgs
    {
        public CrossSetupStateEventArgs(CrossSetupState setupState)
        {
            SetupState = setupState;
        }

        public CrossSetupState SetupState { get; }
    }
}
