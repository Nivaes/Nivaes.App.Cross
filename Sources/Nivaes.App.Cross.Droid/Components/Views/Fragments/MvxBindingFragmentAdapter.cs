using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid;

[RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming.")]
public class MvxBindingFragmentAdapter
    : MvxBaseFragmentAdapter
{
    public IMvxFragmentView? FragmentView => Fragment as IMvxFragmentView;

    public MvxBindingFragmentAdapter(ICrossEventSourceFragment eventSource)
        : base(eventSource)
    {
        if (eventSource is not IMvxFragmentView)
            throw new ArgumentException("eventSource must be an IMvxFragmentView", nameof(eventSource));
    }

    protected override void HandleCreateCalled(object? sender, CrossValueEventArgs<Bundle?>? e)
    {
        // Create is called after Fragment is attached to Activity
        // it's safe to assume that Fragment has activity

        if (Fragment?.Activity is not IMvxAndroidView hostMvxView)
        {
            CrossLoggerHost.GetLogger<MvxBindingFragmentAdapter>().LogWarning("Fragment host for fragment type {FragmentType} is not of type IMvxAndroidView", Fragment?.GetType());
            return;
        }

        // if restoring state, Activity.ViewModel might be null, so a harder mechanism is necessary
        var viewModelType = hostMvxView.ViewModel != null
            ? hostMvxView.ViewModel.GetType()
            : hostMvxView.FindAssociatedViewModelTypeOrNull();

        if (viewModelType == null)
        {
            CrossLoggerHost.GetLogger<MvxBindingFragmentAdapter>().LogWarning(
                "ViewModel type for Activity {FragmentActivityType} not found when trying to show fragment: {FragmentType}",
                Fragment.Activity.GetType(), Fragment.GetType());

            return;
        }

        (Bundle? bundle, CrossViewModelRequest? request) = GetAndroidBundleAndRequest(e);

        var mvxBundle = ReadAndroidBundle(bundle);
        if (FragmentView?.ViewModel == null)
            FragmentView?.OnCreate(mvxBundle, request);
    }

    private (Bundle? bundle, CrossViewModelRequest? request) GetAndroidBundleAndRequest(CrossValueEventArgs<Bundle>? bundleArgs)
    {
        Bundle? bundle = null;
        CrossViewModelRequest? request = null;
        if (bundleArgs?.Value != null)
        {
            // saved state
            bundle = bundleArgs.Value;
        }
        else if (FragmentView is Fragment { Arguments: { } } fragment)
        {
            bundle = fragment.Arguments;
            var json = bundle.GetString("__mvxViewModelRequest");
            if (string.IsNullOrEmpty(json))
                return (bundle, request);

            request = ReadRequest(request, json);
        }

        return (bundle, request);
    }

    private static CrossViewModelRequest? ReadRequest(CrossViewModelRequest? request, string json)
    {
        var serializer = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossNavigationSerializer>();

        request = serializer?.Serializer.DeserializeObject<CrossViewModelRequest>(json);
        return request;
    }

    private static ICrossBundle ReadAndroidBundle(Bundle? bundle)
    {
        var converter = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxSavedStateConverter>();
        if (bundle != null)
            return converter?.Read(bundle) ?? new CrossBundle();

        CrossLoggerHost.GetLogger<MvxBindingFragmentAdapter>().LogWarning(
            "Saved state converter not available - saving state will be hard");

        return new CrossBundle();
    }

    protected override void HandleCreateViewCalled(
        object? sender, CrossValueEventArgs<MvxCreateViewParameters> e) =>
        FragmentView?.EnsureBindingContextIsSet(e.Value.Inflater);

    protected override void HandleResumeCalled(object? sender, EventArgs e)
    {
        var cache = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxMultipleViewModelCache>();
        //if (Mvx.IoCProvider?.TryResolve(out IMvxMultipleViewModelCache? cache) == true && cache != null &&
        if (FragmentView?.ViewModel != null)
        {
            // clear cache if still there
            cache.GetAndClear(FragmentView.ViewModel.GetType(), FragmentView.UniqueImmutableCacheTag);
        }
    }

    protected override void HandleSaveInstanceStateCalled(object? sender, CrossValueEventArgs<Bundle> e)
    {
        // it is guaranteed that SaveInstanceState call will be executed before OnStop (thus before Fragment detach)
        // it is safe to assume that Fragment has activity attached

        var mvxBundle = FragmentView?.CreateSaveStateBundle();
        if (mvxBundle != null)
        {
            var converter = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxSavedStateConverter>();

            converter?.Write(e.Value, mvxBundle);
        }

        if (FragmentView == null)
            return;

        var cache = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IMvxMultipleViewModelCache>();
        cache?.Cache(FragmentView.ViewModel!, FragmentView.UniqueImmutableCacheTag);
    }

    protected override void HandleDestroyViewCalled(object? sender, EventArgs e)
    {
        FragmentView?.BindingContext?.ClearAllBindings();
        base.HandleDestroyViewCalled(sender, e);
    }

    protected override void HandleDisposeCalled(object? sender, EventArgs e)
    {
        FragmentView?.BindingContext?.ClearAllBindings();
        base.HandleDisposeCalled(sender, e);
    }
}
