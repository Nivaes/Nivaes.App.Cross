using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Nivaes.App.Cross.Droid;

public static class MvxFragmentExtensions
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
            //IMvxMultipleViewModelCache? cache = null;

            var cache = IPlatformApplication.Current!.Services.GetRequiredService<IMvxMultipleViewModelCache>();
            //if (Mvx.IoCProvider?.TryResolve(out cache) == true && fragmentView.ViewModel != null)
            if (fragmentView.ViewModel != null)
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

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type FindAssociatedViewModelType(Type fragmentActivityParentType)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();

            var type = fragmentView.GetType();

            if (viewModelType == null)
            {
                if (!type.HasBasePresentationAttribute())
                    throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not have {nameof(MvxFragmentPresentationAttribute)} attribute set!");

                var cacheableFragmentAttribute = type.GetBasePresentationAttribute();
                if (cacheableFragmentAttribute?.ViewModelType == null)
                    throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not use {nameof(MvxFragmentPresentationAttribute)} with ViewModel Type constructor.");

                viewModelType = cacheableFragmentAttribute.ViewModelType;
            }

            return viewModelType;
        }

        public ICrossViewModel? LoadViewModel(ICrossBundle savedState, Type fragmentParentActivityType, CrossViewModelRequest? request = null)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragmentParentActivityType);
            //if (viewModelType == typeof(CrossNullViewModel))
            //    return new CrossNullViewModel();

            if (viewModelType == null)
                return null;

            if (viewModelType == null
                || viewModelType == typeof(ICrossViewModel))
            {
                CrossLoggerHost.GetLogger(nameof(MvxFragmentExtensions)).Log(LogLevel.Trace,
                    "No ViewModel class specified for {FragmentViewType} in LoadViewModel",
                    fragmentView.GetType().Name);
            }

            if (request == null)
                request = CrossViewModelRequest.GetDefaultRequest(viewModelType!);

            var viewModelCache = IPlatformApplication.Current!.Services.GetRequiredService<ICrossChildViewModelCache>();
            if (viewModelCache.Exists(viewModelType!))
            {
                var viewModelCached = viewModelCache.Get(viewModelType!);
                viewModelCache.Remove(viewModelType!);
                return viewModelCached!;
            }

            var loaderService = IPlatformApplication.Current!.Services.GetRequiredService<ICrossViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(request, savedState);

            return viewModel;
        }
    
        public void EnsureBindingContextIsSet(LayoutInflater inflater)
        {
            var actualFragment = fragmentView.ToFragment();
            if (actualFragment == null)
                throw new CrossException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

            if (fragmentView.BindingContext == null)
            {
                fragmentView.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity!,
                    new MvxSimpleLayoutInflaterHolder(inflater),
                    fragmentView.DataContext);
            }
            else if (fragmentView.BindingContext is IMvxAndroidBindingContext androidContext)
            {
                androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(inflater);
            }
        }

        public void EnsureBindingContextIsSet()
        {
            var actualFragment = fragmentView.ToFragment();
            if (actualFragment == null)
                throw new CrossException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

            if (fragmentView.BindingContext == null)
            {
                fragmentView.BindingContext = new MvxAndroidBindingContext(actualFragment.Context!,
                    new MvxSimpleLayoutInflaterHolder(
                        actualFragment.LayoutInflater),
                    fragmentView.DataContext);
            }
            else if (fragmentView.BindingContext is IMvxAndroidBindingContext androidContext)
            {
                androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(actualFragment.LayoutInflater);
            }
        }

        public void LoadViewModelFrom(CrossViewModelRequest request, ICrossBundle? savedState = null)
        {
            var loader = IPlatformApplication.Current!.Services.GetRequiredService<ICrossViewModelLoader>();

            //if (Mvx.IoCProvider?.TryResolve(out ICrossViewModelLoader? loader) != true)
            //    return;

            var viewModel = loader?.LoadViewModel(request, savedState);
            if (viewModel == null)
            {
                CrossLoggerHost.GetLogger(nameof(MvxFragmentExtensions)).LogWarning("ViewModel not loaded for {ViewModelType}",
                    request.ViewModelType?.FullName);
                return;
            }

            fragmentView.ViewModel = viewModel;
        }
    }

    extension(ICrossActivity activity)
    {
        public TFragment? FindFragmentById<TFragment>(int resourceId)
        where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentById(resourceId);
            if (fragment == null)
            {
                CrossLoggerHost.GetLogger(nameof(MvxFragmentExtensions)).LogWarning(
                    "Failed to find fragment id {ResourceId} in {ActivityTypeName}", resourceId, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        public TFragment? FindFragmentByTag<TFragment>(string tag)
            where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentByTag(tag);
            if (fragment == null)
            {
                CrossLoggerHost.GetLogger(nameof(MvxFragmentExtensions))?.LogWarning(
                    "Failed to find fragment tag {Tag} in {ActivityTypeName}", tag, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }
    }

    extension(Fragment fragment)
    {
        private TFragment? SafeCast<TFragment>() where TFragment : Fragment
        {
            if (fragment is TFragment castFragment)
                return castFragment;

            CrossLoggerHost.GetLogger(nameof(MvxFragmentExtensions)).LogWarning(
                "Fragment type mismatch got {FragmentType} but expected {ExpectedType}",
                fragment.GetType().FullName, typeof(TFragment).FullName);
            return default;
        }
    }

    extension(ICrossViewModel viewModel)
    {
        public void RunViewModelLifecycle(ICrossBundle savedState,
        CrossViewModelRequest request)
        {
            try
            {
                if (request != null)
                {
                    var parameterValues = new CrossBundle(request.ParameterValues);
                    viewModel.CallBundleMethods("Init", parameterValues);
                }
                if (savedState != null)
                {
                    viewModel.CallBundleMethods("ReloadState", savedState);
                }
                viewModel.Start();
            }
            catch (Exception ex)
            {
                throw new CrossException(ex, "Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }
    }

    extension(Type fragmentType)
    {
        public string FragmentJavaName()
        {
            return Java.Lang.Class.FromType(fragmentType).Name;
        }
    }
}