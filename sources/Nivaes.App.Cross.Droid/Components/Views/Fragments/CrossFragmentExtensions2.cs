namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;
    using AndroidX.Fragment.App;
    using Microsoft.Extensions.Logging;
    using Fragment = AndroidX.Fragment.App.Fragment;

    // ToDo: Mirar por que está repetido CrossFragmentExtensions. Unificarlas o cambiar a una de nombre.
    public static class CrossFragmentExtensions2
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static void AddEventListeners(this ICrossEventSourceFragment fragment)
        {
            if (fragment is ICrossFragmentView) 
            {
                var adapter = new CrossBindingFragmentAdapter(fragment);
            }
        }

        public static void OnCreate(this ICrossFragmentView fragmentView, ICrossBundle bundle, ICrossViewModelRequest? request = null)
        {
            throw new NotImplementedException();    

            //ICrossMultipleViewModelCache? cache = null;
            //if (Mvx.IoCProvider?.TryResolve(out cache) == true && fragmentView.ViewModel != null)
            //{
            //    // check if ViewModel instance was cached. If so, clear it and ignore previous instance
            //    cache!.GetAndClear(fragmentView.ViewModel.GetType(), fragmentView.UniqueImmutableCacheTag);
            //    return;
            //}

            //var fragment = fragmentView.ToFragment();
            //if (fragment == null)
            //    throw new CrossException($"{nameof(OnCreate)} called on an {nameof(ICrossFragmentView)} which is not an Android Fragment: {fragmentView}");

            //if (fragment.Activity == null)
            //    return;

            //// as it is called during onCreate it is safe to assume that fragment has Activity attached.
            //var viewModelType = fragmentView.FindAssociatedViewModelType(fragment.Activity.GetType());
            //var view = fragmentView as ICrossView;

            //var cached = cache?.GetAndClear(viewModelType, fragmentView.UniqueImmutableCacheTag);
            //view.OnViewCreate(() => cached ?? fragmentView.LoadViewModel(bundle, fragment.Activity.GetType(), request));
        }

        public static Fragment? ToFragment(this ICrossFragmentView fragmentView)
        {
            return fragmentView as Fragment;
        }

        public static void EnsureBindingContextIsSet(this ICrossFragmentView fragment, LayoutInflater inflater)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new CrossException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(ICrossFragmentView)} which is not an Android Fragment: {fragment}");

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new CrossAndroidBindingContext(actualFragment.Activity,
                    new CrossSimpleLayoutInflaterHolder(inflater),
                    fragment.DataContext);
            }
            else if (fragment.BindingContext is ICrossAndroidBindingContext androidContext)
            {
                androidContext.LayoutInflaterHolder = new CrossSimpleLayoutInflaterHolder(inflater);
            }
        }

        public static void EnsureBindingContextIsSet(this ICrossFragmentView fragment)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new CrossException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(ICrossFragmentView)} which is not an Android Fragment: {fragment}");

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new CrossAndroidBindingContext(actualFragment.Context,
                    new CrossSimpleLayoutInflaterHolder(
                        actualFragment.LayoutInflater),
                    fragment.DataContext);
            }
            else if (fragment.BindingContext is ICrossAndroidBindingContext androidContext)
            {
                androidContext.LayoutInflaterHolder = new CrossSimpleLayoutInflaterHolder(actualFragment.LayoutInflater);
            }
        }

        public static TFragment? FindFragmentById<TFragment>(this ICrossActivity activity, int resourceId)
            where TFragment : Fragment
        {
            var fragment = ((FragmentActivity)activity).SupportFragmentManager.FindFragmentById(resourceId);
            if (fragment == null)
            {
                CrossLogHost.Default?.Log(LogLevel.Warning,
                    "Failed to find fragment id {ResourceId} in {ActivityTypeName}", resourceId, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        public static TFragment? FindFragmentByTag<TFragment>(this ICrossActivity activity, string tag)
            where TFragment : Fragment
        {
            var fragment = ((FragmentActivity)activity).SupportFragmentManager.FindFragmentByTag(tag);
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

        public static void LoadViewModelFrom(this ICrossFragmentView view, ICrossViewModelRequest request, ICrossBundle? savedState = null)
        {
            throw new NotImplementedException();
            //if (Mvx.IoCProvider?.TryResolve(out ICrossViewModelLoader? loader) != true)
            //    return;

            //var viewModel = loader?.LoadViewModel(request, savedState);
            //if (viewModel == null)
            //{
            //    CrossLogHost.Default?.Log(LogLevel.Warning, "ViewModel not loaded for {ViewModelType}",
            //        request.ViewModelType?.FullName);
            //    return;
            //}

            //view.ViewModel = viewModel;
        }
    }
}