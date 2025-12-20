using Android.Content;

namespace MvvmCross.Platforms.Android.Views
{
    using System.Diagnostics.CodeAnalysis;    
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(MvxViewModelRequest request);

        // Important: if calling GetIntentWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        (Intent intent, int key) GetIntentWithKeyFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
            TViewModel existingViewModelToUse, MvxViewModelRequest? request)
                where TViewModel : ICrossViewModel;

        void RemoveSubViewModelWithKey(int key);
    }
}
