using System.Security.Cryptography;
using System.Text;
using Android.Content;
using static Android.Provider.Settings;

namespace Nivaes.App.Cross.Droid
{

    public class DeviceService
        : IDeviceService
    {
        private Context mContext;

        public DeviceService(Context context)
        {
            mContext = context;
        }

        byte[] IDeviceService.GetUniqueIdentifier()
        {
            var stringId = Secure.GetString(mContext.ContentResolver, Secure.AndroidId);

            var buffer = Encoding.UTF8.GetBytes(stringId!);

            byte[] hash = MD5.Create().ComputeHash(buffer);

            return hash;
        }

        string IDeviceService.GetVersionApp()
        {
            return mContext.PackageManager!.GetPackageInfo(mContext.PackageName!, 0)!.VersionName!;
        }

        void IDeviceService.RestartApp()
        { }
    }
}
