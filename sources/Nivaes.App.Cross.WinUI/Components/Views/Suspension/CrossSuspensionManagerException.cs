namespace Nivaes.App.Cross.WinUI
{
    using System;

    public class CrossSuspensionManagerException
        : CrossException
    {
        public CrossSuspensionManagerException()
        {
        }

        public CrossSuspensionManagerException(Exception e)
            : base(e, "MvxSuspensionManager failed")
        {
        }
    }
}
