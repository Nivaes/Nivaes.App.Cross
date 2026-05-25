namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;

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
