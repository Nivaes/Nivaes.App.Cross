using System.Security.Cryptography;

namespace Nivaes.App.Cross.UIKitLib
{
    public class DeviceService
        : IDeviceService
    {
        public DeviceService()
        {
        }

        byte[] IDeviceService.GetUniqueIdentifier()
        {
            var id = UIDevice.CurrentDevice.IdentifierForVendor!.GetBytes();

            byte[] hash = MD5.Create().ComputeHash(id);

            return hash;
        }

        string IDeviceService.GetVersionApp()
        {
            throw new NotImplementedException();
        }

        void IDeviceService.RestartApp()
        { }
    }
}
