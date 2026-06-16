using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MvxTabLayoutPresentationAttribute : MvxViewPagerFragmentPresentationAttribute
{
    public MvxTabLayoutPresentationAttribute()
    {
    }

    public MvxTabLayoutPresentationAttribute(
        string title,
        int viewPagerResourceId,
        int tabLayoutResourceId,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? activityHostViewModelType = null,
        bool addToBackStack = false,
        Type? fragmentHostViewType = null,
        bool isCacheableFragment = false)
        : base(
              title,
              viewPagerResourceId,
              activityHostViewModelType,
              addToBackStack,
              fragmentHostViewType,
              isCacheableFragment)
    {
        TabLayoutResourceId = tabLayoutResourceId;
    }

    public MvxTabLayoutPresentationAttribute(
        string title,
        string viewPagerResourceName,
        string tabLayoutResourceName,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? activityHostViewModelType = null,
        bool addToBackStack = false,
        Type? fragmentHostViewType = null,
        bool isCacheableFragment = false)
        : base(
              title,
              viewPagerResourceName,
              activityHostViewModelType,
              addToBackStack,
              fragmentHostViewType,
              isCacheableFragment)
    {
        var globals = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidGlobals>();

        if (!string.IsNullOrEmpty(tabLayoutResourceName) &&
            //Mvx.IoCProvider?.TryResolve(out IMvxAndroidGlobals globals) == true &&
            globals.ApplicationContext.Resources != null)
        {
            TabLayoutResourceId = globals.ApplicationContext.Resources.GetIdentifier(
                tabLayoutResourceName, "id", globals.ApplicationContext.PackageName);
        }
        else
        {
            TabLayoutResourceId = global::Android.Resource.Id.Content;
        }
    }

    /// <summary>
    ///     The resource used to get the TabLayout from the view
    /// </summary>
    public int TabLayoutResourceId { get; set; }
}