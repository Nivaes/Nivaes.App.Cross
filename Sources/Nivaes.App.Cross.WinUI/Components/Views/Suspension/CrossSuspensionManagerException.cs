namespace Nivaes.App.Cross.WinUI
{
    using System;

    public class CrossSuspensionManagerException
        : AppException
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
