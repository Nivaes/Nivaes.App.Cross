using Android.Content;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(IViewModelRequest request);

        // Important: if calling GetIntentWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        (Intent intent, uint requestId) GetIntentWithKeyFor<TViewModel>(
            TViewModel existingViewModelToUse, IViewModelRequest? request)
                where TViewModel : ICrossViewModel;

        void RemoveSubViewModelWithKey(uint requestId);
    }
}
