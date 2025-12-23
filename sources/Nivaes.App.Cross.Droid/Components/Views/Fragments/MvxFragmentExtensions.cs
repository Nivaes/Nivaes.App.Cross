namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Logging;
    using MvvmCross.Platforms.Android.Views;
    using MvvmCross.Platforms.Android.Views.Fragments;
    using Nivaes.App.Cross;
    using Fragment = AndroidX.Fragment.App.Fragment;

    public static class MvxFragmentExtensions2
    {
        extension(ICrossEventSourceFragment fragment)
        {
            [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
            public void AddEventListeners()
            {
                if (fragment is IMvxFragmentView)
                {
                    var adapter = new MvxBindingFragmentAdapter(fragment);
                }
            }
        }

        extension(IMvxFragmentView fragmentView)
        {
            public void OnCreate(ICrossBundle bundle, CrossViewModelRequest? request = null)
            {
                IMvxMultipleViewModelCache? cache = null;
                if (Mvx.IoCProvider?.TryResolve(out cache) == true && fragmentView.ViewModel != null)
                {
                    // check if ViewModel instance was cached. If so, clear it and ignore previous instance
                    cache!.GetAndClear(fragmentView.ViewModel.GetType(), fragmentView.UniqueImmutableCacheTag);
                    return;
                }

                var fragment = fragmentView.ToFragment();
                if (fragment == null)
                    throw new CrossException($"{nameof(OnCreate)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

                if (fragment.Activity == null)
                    return;

                // as it is called during onCreate it is safe to assume that fragment has Activity attached.
                var viewModelType = fragmentView.FindAssociatedViewModelType(fragment.Activity.GetType());
                var view = fragmentView as ICrossView;

                var cached = cache?.GetAndClear(viewModelType, fragmentView.UniqueImmutableCacheTag);
                view.OnViewCreate(() => cached ?? fragmentView.LoadViewModel(bundle, fragment.Activity.GetType(), request));
            }

            public Fragment? ToFragment()
            {
                return fragmentView as Fragment;
            }
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, LayoutInflater inflater)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new CrossException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragment}");

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity,
                    new MvxSimpleLayoutInflaterHolder(inflater),
                    fragment.DataContext);
            }
            else if (fragment.BindingContext is IMvxAndroidBindingContext androidContext)
            {
                androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(inflater);
            }
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new CrossException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragment}");

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Context,
                    new MvxSimpleLayoutInflaterHolder(
                        actualFragment.LayoutInflater),
                    fragment.DataContext);
            }
            else if (fragment.BindingContext is IMvxAndroidBindingContext androidContext)
            {
                androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(actualFragment.LayoutInflater);
            }
        }

        public static TFragment? FindFragmentById<TFragment>(this MvxActivity activity, int resourceId)
            where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentById(resourceId);
            if (fragment == null)
            {
                CrossLogHost.Default?.Log(LogLevel.Warning,
                    "Failed to find fragment id {ResourceId} in {ActivityTypeName}", resourceId, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        public static TFragment? FindFragmentByTag<TFragment>(this MvxActivity activity, string tag)
            where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentByTag(tag);
            if (fragment == null)
            {
                CrossLogHost.Default?.Log(LogLevel.Warning,
                    "Failed to find fragment tag {Tag} in {ActivityTypeName}", tag, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        private static TFragment? SafeCast<TFragment>(Fragment fragment) where TFragment : Fragment
        {
            if (fragment is TFragment castFragment)
                return castFragment;

            CrossLogHost.Default?.Log(LogLevel.Warning,
                "Fragment type mismatch got {FragmentType} but expected {ExpectedType}",
                fragment.GetType().FullName, typeof(TFragment).FullName);
            return default;
        }

        public static void LoadViewModelFrom(this IMvxFragmentView view, CrossViewModelRequest request, ICrossBundle? savedState = null)
        {
            if (Mvx.IoCProvider?.TryResolve(out ICrossViewModelLoader? loader) != true)
                return;

            var viewModel = loader?.LoadViewModel(request, savedState);
            if (viewModel == null)
            {
                CrossLogHost.Default?.Log(LogLevel.Warning, "ViewModel not loaded for {ViewModelType}",
                    request.ViewModelType?.FullName);
                return;
            }

            view.ViewModel = viewModel;
        }
    }
}