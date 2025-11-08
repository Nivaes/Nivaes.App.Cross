namespace Nivaes.App.Cross.Droid
{
    using Android.Content;
    public interface ICrossStartActivityForResult
    {
        void MvxInternalStartActivityForResult(Intent intent, int requestCode);
    }
}
