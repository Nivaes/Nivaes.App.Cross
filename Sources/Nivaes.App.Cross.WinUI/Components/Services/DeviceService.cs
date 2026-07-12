using Windows.ApplicationModel.Core;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;

namespace Nivaes.App.Cross.WinUI;

public class DeviceService
    : IDeviceService
{
    public DeviceService()
    {
    }

    byte[] IDeviceService.GetUniqueIdentifier()
    {
        if (Windows.Foundation.Metadata.ApiInformation
                               .IsTypePresent("Windows.System.Profile.HardwareIdentification"))
        {
            HardwareToken token = HardwareIdentification.GetPackageSpecificToken(null);
            IBuffer hardwareId = token.Id;

            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer hashed = hasher.HashData(hardwareId);

            CryptographicBuffer.CopyToByteArray(hashed, out byte[] buffer);

            return buffer;
        }

        return null;
    }

    string IDeviceService.GetVersionApp()
    {
        var v = Windows.ApplicationModel.Package.Current.Id.Version;
        return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
    }

    async void IDeviceService.RestartApp()
    {
        await CoreApplication.RequestRestartAsync("");
    }
}
