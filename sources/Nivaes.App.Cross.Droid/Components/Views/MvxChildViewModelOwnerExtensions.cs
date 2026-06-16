using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid
{
    public static class MvxChildViewModelOwnerExtensions
    {
        public static Intent CreateIntentFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(this IMvxAndroidView view, object parameterObject)
            where TTargetViewModel : class, ICrossViewModel
        {
            return view.CreateIntentFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());
        }

        public static Intent CreateIntentFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
                this IMvxAndroidView view,
                IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, ICrossViewModel
        {
            var parameterBundle = new CrossBundle(parameterValues);
            var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxAndroidView view, CrossViewModelRequest request)
        {
            return IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        public static Intent CreateIntentFor(this IMvxChildViewModelOwner view, ICrossViewModel subViewModel)
        {
            var requestTranslator = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewModelRequestTranslator>();
            var (intent, key) = requestTranslator.GetIntentWithKeyFor(subViewModel, null);

            view.OwnedSubViewModelIndicies.Add(key);

            return intent;
        }

        public static void ClearOwnedSubIndicies(this IMvxChildViewModelOwner view)
        {
            var translator = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidViewModelRequestTranslator>();
            foreach (var ownedSubViewModelIndex in view.OwnedSubViewModelIndicies)
            {
                translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            }
            view.OwnedSubViewModelIndicies.Clear();
        }
    }
}
