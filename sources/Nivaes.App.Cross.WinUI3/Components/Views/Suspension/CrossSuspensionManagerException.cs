namespace Nivaes.App.Cross.WinUI3
{
    public class CrossSuspensionManagerException : CrossException
    {
        public CrossSuspensionManagerException()
        {
        }

        public CrossSuspensionManagerException(Exception e)
            : base(e, "CrossSuspensionManager failed")
        {
        }
    }
}
