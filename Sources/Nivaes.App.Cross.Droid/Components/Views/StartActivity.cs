using Android.Runtime;
using Android.Views;
using AndroidX.Lifecycle;
using Microsoft.Extensions.DependencyInjection;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;

namespace Nivaes.App.Cross.Droid;

public abstract class StartActivity
    : Activity
{
    protected const int NoContent = 0;

    private readonly int _resourceId;

    private Bundle? _bundle;

    protected StartActivity(int resourceId = NoContent)
    {
        _resourceId = resourceId;
    }

    protected StartActivity(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected virtual void RequestWindowFeatures()
    {
        RequestWindowFeature(WindowFeatures.NoTitle);
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        RequestWindowFeatures();

        _bundle = savedInstanceState;

        base.OnCreate(savedInstanceState);

        if (_resourceId != NoContent)
        {
            // Set our view from the "splash" layout resource
            // Be careful to use non-binding inflation
            var content = LayoutInflater.Inflate(_resourceId, null);
            SetContentView(content);
        }
    }

    protected override async void OnResume()
    {
        base.OnResume();

        IPlatformApplication.Current!.Application.Setup();
        IPlatformApplication.Current!.Application.Initialize();
    }

    protected virtual object? GetAppStartHint(object? hint = null)
    {
        return hint;
    }
}