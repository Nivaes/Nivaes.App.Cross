using System.Diagnostics.CodeAnalysis;
using AndroidX.Core.ViewTree;
using Java.Lang;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid
{
    public abstract class PressenterAction<TPressenterAttribute>
        : Cross.PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        #region Properties
        public IPressenterActionContext Context { get; }

        protected IViewModelRequest? PendingRequest { get; set; }
        #endregion

        #region Constructor
        protected PressenterAction(
                IPressenterActionContext contex,
                ILogger logger)
            : base(logger)
        {
            Context = contex;
        }
        #endregion

        protected override BasePresentationAttribute CreatePresentationAttribute(IViewModelRequest request)
        {
            if (request.ViewType.IsSubclassOf(typeof(DialogFragment)))
            {
                Logger.LogWarning($"PresentationAttribute not found for {request.ViewType.Name}. Assuming DialogFragment presentation");
                return new DialogFragmentPresentationAttribute(enterAnimation: int.MinValue);
            }

            if (request.ViewType.IsSubclassOf(typeof(Fragment)))
            {
                Logger.LogWarning($"PresentationAttribute not found for {request.ViewType.Name}. Assuming Fragment presentation");
                return new FragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content);
            }

            if (request.ViewType.IsSubclassOf(typeof(Activity)))
            {
                Logger.LogWarning($"PresentationAttribute not found for {request.ViewType.Name}. Assuming Activity presentation");
                return new ActivityPresentationAttribute();
            }

            throw new InvalidOperationException($"Don't know how to create a presentation attribute for type {{request.ViewType.Name}}");
        }

        protected Type? GetCurrentActivityViewModelType()
        {
            Type? currentActivityType = null;
            if (Context.CurrentActivity!.IsActivityAlive())
                currentActivityType = Context.CurrentActivity!.GetType();

            if (currentActivityType == null)
                return null;

            Singleton<ViewsContainers>.Instance.ViewViewModels.TryGetValue(currentActivityType, out var viewModelType);

            return viewModelType;
        }

        protected virtual async void ShowHostActivity(FragmentPresentationAttribute attribute)
        {
            if (attribute.ActivityHostViewModelType == null)
                throw new ArgumentException("ActivityHostViewModelType not set on attribute");

             var viewType = Singleton<ViewsContainers>.Instance.ViewModelViews[attribute.ActivityHostViewModelType];
                

            if (viewType?.IsSubclassOf(typeof(Activity)) != true)
                throw new AppException("The host activity doesn't inherit Activity");

            var hostViewModelRequest = ViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            if (PendingRequest != null)
                hostViewModelRequest.PresentationValues = PendingRequest.PresentationValues;

            await Show(hostViewModelRequest);
        }

        public virtual void OnFragmentChanged(FragmentTransaction? fragmentTransaction, Fragment? fragment, 
            FragmentPresentationAttribute? attribute, IViewModelRequest? request)
        {
            if (fragment is IBaseMasterDetailView masterDetailView)
            {
                masterDetailView.ActivityCreated += MasterDetailViewActivityCreated;
            }
        }

        private void MasterDetailViewActivityCreated(object? sender, EventArgs e)
        {
            ((IBaseMasterDetailView?)sender)?.ActivityCreated -= MasterDetailViewActivityCreated;

            if (Context.PendingDetailFragmentRequests != null)
            {
                base.Show(Context.PendingDetailFragmentRequests);
                Context.PendingDetailFragmentRequests = null;
            }
            if (Context.PendingDefaultDetailRequests != null)
            {
                base.Show(Context.PendingDefaultDetailRequests);
                Context.PendingDefaultDetailRequests = null;
            }
        }

    }
}
