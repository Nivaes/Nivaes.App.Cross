namespace Nivaes.App.Cross.Droid
{
    using Android.App;
    using Android.Content;

    public class CrossActivityResultParameters
    {
        public CrossActivityResultParameters(int requestCode, Result resultCode, Intent? data)
        {
            Data = data;
            ResultCode = resultCode;
            RequestCode = requestCode;
        }

        public int RequestCode { get; private set; }
        public Result ResultCode { get; private set; }
        public Intent? Data { get; private set; }
    }
}
