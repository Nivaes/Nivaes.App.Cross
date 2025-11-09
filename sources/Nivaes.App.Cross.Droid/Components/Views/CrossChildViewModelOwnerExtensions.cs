namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content;

    public static class CrossChildViewModelOwnerExtensions
    {
        public static Intent CreateIntentFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(this ICrossAndroidView view, object parameterObject)
            where TTargetViewModel : class, ICrossViewModel
        {
            return view.CreateIntentFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());
        }

        public static Intent CreateIntentFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
        this ICrossAndroidView view,
        IDictionary<string, string> parameterValues = null)
        where TTargetViewModel : class, ICrossViewModel
        {
            var parameterBundle = new CrossBundle(parameterValues);
            var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateIntentFor(request);
        }

        public static Intent CreateIntentFor(this ICrossAndroidView view, ICrossViewModelRequest request)
        {
            throw new NotImplementedException();
            //return Mvx.IoCProvider.Resolve<ICrossAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        public static Intent CreateIntentFor(this ICrossChildViewModelOwner view, ICrossViewModel subViewModel)
        {
            throw new NotImplementedException();
            //var requestTranslator = Mvx.IoCProvider.Resolve<ICrossAndroidViewModelRequestTranslator>();
            //var (intent, key) = requestTranslator.GetIntentWithKeyFor(subViewModel, null);

            //view.OwnedSubViewModelIndicies.Add(key);

            //return intent;
        }

        public static void ClearOwnedSubIndicies(this ICrossChildViewModelOwner view)
        {
            throw new NotImplementedException();
            //var translator = Mvx.IoCProvider.Resolve<ICrossAndroidViewModelRequestTranslator>();
            //foreach (var ownedSubViewModelIndex in view.OwnedSubViewModelIndicies)
            //{
            //    translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            //}
            //view.OwnedSubViewModelIndicies.Clear();
        }
    }
}
