// Licensed to the .NET Foundation under one or more agreements.
namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content;

    public interface ICrossAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(ICrossViewModelRequest request);

        // Important: if calling GetIntentWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        (Intent intent, int key) GetIntentWithKeyFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
            TViewModel existingViewModelToUse, ICrossViewModelRequest? request)
                where TViewModel : ICrossViewModel;

        void RemoveSubViewModelWithKey(int key);
    }
}
