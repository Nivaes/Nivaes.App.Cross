namespace Nivaes.App.Cross.Droid
{
    using Android.Content;

    public class CrossStartActivityForResultParameters
    {
        public CrossStartActivityForResultParameters(Intent? intent, int requestCode)
        {
            RequestCode = requestCode;
            Intent = intent;
        }

        public Intent? Intent { get; private set; }
        public int RequestCode { get; private set; }
    }
}
