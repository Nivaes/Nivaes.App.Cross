using Android.Content;

namespace Nivaes.App.Cross.Droid
{
    public record MvxActivityResultParameters
    {
        public MvxActivityResultParameters(int requestCode, Android.App.Result resultCode, Intent? data)
        {
            Data = data;
            ResultCode = resultCode;
            RequestCode = requestCode;
        }

        public int RequestCode { get; private set; }
        public Android.App.Result ResultCode { get; private set; }
        public Intent? Data { get; private set; }
    }
}
