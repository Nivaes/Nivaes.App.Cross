using Android.Content;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public interface IMvxAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(CrossViewModelRequest request);

        // Important: if calling GetIntentWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        (Intent intent, int key) GetIntentWithKeyFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
            TViewModel existingViewModelToUse, CrossViewModelRequest? request)
                where TViewModel : ICrossViewModel;

        void RemoveSubViewModelWithKey(int key);
    }
}
