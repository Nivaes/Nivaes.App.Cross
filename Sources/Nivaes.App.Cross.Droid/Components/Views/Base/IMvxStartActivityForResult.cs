using Android.Content;

namespace Nivaes.App.Cross.Droid;

public interface IMvxStartActivityForResult
{
    void MvxInternalStartActivityForResult(Intent intent, int requestCode);
}
