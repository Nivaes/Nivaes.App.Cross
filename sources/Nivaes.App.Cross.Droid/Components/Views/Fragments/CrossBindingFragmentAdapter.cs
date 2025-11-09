namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Fragment = AndroidX.Fragment.App.Fragment;

    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming.")]
    public class CrossBindingFragmentAdapter
        : CrossBaseFragmentAdapter
    {
        public ICrossFragmentView? FragmentView => Fragment as ICrossFragmentView;

        public CrossBindingFragmentAdapter(ICrossEventSourceFragment eventSource)
            : base(eventSource)
        {
            if (eventSource is not ICrossFragmentView)
                throw new ArgumentException("eventSource must be an IMvxFragmentView", nameof(eventSource));
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        protected override void HandleCreateCalled(object? sender, CrossValueEventArgs<Bundle>? e)
        {
            // Create is called after Fragment is attached to Activity
            // it's safe to assume that Fragment has activity

            if (Fragment?.Activity is not ICrossAndroidView hostMvxView)
            {
                CrossLogHost.GetLog<CrossBindingFragmentAdapter>()?.Log(LogLevel.Warning,
                    "Fragment host for fragment type {FragmentType} is not of type IMvxAndroidView", Fragment?.GetType());
                return;
            }

            // if restoring state, Activity.ViewModel might be null, so a harder mechanism is necessary
            var viewModelType = hostMvxView.ViewModel != null
                ? hostMvxView.ViewModel.GetType()
                : hostMvxView.FindAssociatedViewModelTypeOrNull();

            if (viewModelType == null)
            {
                CrossLogHost.GetLog<CrossBindingFragmentAdapter>()?.Log(LogLevel.Warning,
                    "ViewModel type for Activity {FragmentActivityType} not found when trying to show fragment: {FragmentType}",
                    Fragment.Activity.GetType(), Fragment.GetType());
                return;
            }

            (Bundle? bundle, ICrossViewModelRequest? request) = GetAndroidBundleAndRequest(e);

            var mvxBundle = ReadAndroidBundle(bundle);
            if (FragmentView?.ViewModel == null)
                FragmentView?.OnCreate(mvxBundle, request);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        private (Bundle? bundle, ICrossViewModelRequest? request) GetAndroidBundleAndRequest(CrossValueEventArgs<Bundle>? bundleArgs)
        {
            Bundle? bundle = null;
            ICrossViewModelRequest? request = null;
            if (bundleArgs?.Value != null)
            {
                // saved state
                bundle = bundleArgs.Value;
            }
            else if (FragmentView is Fragment { Arguments: { } } fragment)
            {
                bundle = fragment.Arguments;
                var json = bundle.GetString("__CrossViewModelRequest");
                if (string.IsNullOrEmpty(json))
                    return (bundle, request);

                request = ReadRequest(request, json);
            }

            return (bundle, request);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        private static ICrossViewModelRequest? ReadRequest(ICrossViewModelRequest? request, string json)
        {
            throw new NotImplementedException();
            //if (Mvx.IoCProvider?.TryResolve(out IMvxNavigationSerializer? serializer) == true)
            //{
            //    request = serializer?.Serializer.DeserializeObject<CrossViewModelRequest>(json);
            //}
            //else
            //{
            //    CrossLogHost.GetLog<CrossBindingFragmentAdapter>()?.Log(LogLevel.Warning,
            //        "Navigation Serializer not available, deserializing ViewModel Request will be hard");
            //}

            //return request;
        }

        private static ICrossBundle ReadAndroidBundle(Bundle? bundle)
        {
            throw new NotImplementedException();
            //if (Mvx.IoCProvider?.TryResolve(out IMvxSavedStateConverter? converter) == true && bundle != null)
            //{
            //    return converter?.Read(bundle) ?? new MvxBundle();
            //}

            //CrossLogHost.GetLog<CrossBindingFragmentAdapter>()?.Log(LogLevel.Warning,
            //"Saved state converter not available - saving state will be hard");

            //return new MvxBundle();
        }

        protected override void HandleCreateViewCalled(
            object? sender, CrossValueEventArgs<CrossCreateViewParameters> e) =>
            FragmentView?.EnsureBindingContextIsSet(e.Value.Inflater);

        protected override void HandleResumeCalled(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
            //if (Mvx.IoCProvider?.TryResolve(out ICrossMultipleViewModelCache? cache) == true && cache != null && FragmentView?.ViewModel != null)
            //{
            //    // clear cache if still there
            //    cache.GetAndClear(FragmentView.ViewModel.GetType(), FragmentView.UniqueImmutableCacheTag);
            //}
        }

        protected override void HandleSaveInstanceStateCalled(object? sender, CrossValueEventArgs<Bundle> e)
        {
            // it is guaranteed that SaveInstanceState call will be executed before OnStop (thus before Fragment detach)
            // it is safe to assume that Fragment has activity attached

            throw new NotImplementedException();

            //var mvxBundle = FragmentView?.CreateSaveStateBundle();
            //if (mvxBundle != null)
            //{
            //    if (Mvx.IoCProvider?.TryResolve(out IMvxSavedStateConverter? converter) != true)
            //    {
            //        CrossLogHost.GetLog<CrossBindingFragmentAdapter>()?.Log(LogLevel.Warning,
            //            "Saved state converter not available - saving state will be hard");
            //    }
            //    else
            //    {
            //        converter?.Write(e.Value, mvxBundle);
            //    }
            //}

            //if (FragmentView == null)
            //    return;

            //if (Mvx.IoCProvider?.TryResolve(out ICrossMultipleViewModelCache? cache) == true)
            //    cache?.Cache(FragmentView.ViewModel, FragmentView.UniqueImmutableCacheTag);
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
}
