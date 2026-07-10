using Android.Runtime;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Droid;

[Register("nivaes.cross.StartActivity")]
public abstract class CrossStartActivity
    : CrossActivity<CrossStartActivityViewModel>
{
    protected const int NoContent = 0;

    private readonly int _resourceId;

    private Bundle? _bundle;

    //public new CrossNullViewModel ViewModel
    //{
    //    get { return base.ViewModel as CrossNullViewModel; }
    //    set { base.ViewModel = value; }
    //}

    protected CrossStartActivity(int resourceId = NoContent)
    {
        //RegisterSetup();
        _resourceId = resourceId;
    }

    protected CrossStartActivity(IntPtr javaReference, JniHandleOwnership transfer)
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
        //await RunAppStartAsync(_bundle);

        IPlatformApplication.Current!.Application.Setup();
        var initializeViewModelType = IPlatformApplication.Current!.Application.Initialize();

        var navigationService = IPlatformApplication.Current!.Services.GetRequiredService<ICrossNavigationService>();
        await initializeViewModelType.NavigateToFirstViewModel(navigationService);
    }

    //protected virtual async Task RunAppStartAsync(Bundle bundle)
    //{
    //    if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart startup) == true)
    //    {
    //        if (!startup.IsStarted)
    //        {
    //            await startup.StartAsync(GetAppStartHint(bundle));
    //        }
    //        else
    //        {
    //            Finish();
    //        }
    //    }
    //}

    protected virtual object? GetAppStartHint(object? hint = null)
    {
        return hint;
    }

    //protected virtual void RegisterSetup()
    //{
    //}
}