using System.Diagnostics.CodeAnalysis;
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

        protected ViewModelRequest? PendingRequest { get; set; }
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

        protected override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            if (viewType!.IsSubclassOf(typeof(DialogFragment)))
            {
                Logger.Log(LogLevel.Trace, "PresentationAttribute not found for {ViewName}. Assuming DialogFragment presentation", viewType.Name);
                return new DialogFragmentPresentationAttribute(enterAnimation: int.MinValue)
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                Logger.LogTrace("PresentationAttribute not found for {ViewName}. Assuming Fragment presentation", viewType.Name);
                return new FragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content)
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            if (viewType.IsSubclassOf(typeof(Activity)))
            {
                Logger.LogTrace("PresentationAttribute not found for {ViewName}. Assuming Activity presentation", viewType.Name);
                return new ActivityPresentationAttribute
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            throw new InvalidOperationException($"Don't know how to create a presentation attribute for type {viewType}");
        }

        protected Type? GetCurrentActivityViewModelType()
        {
            Type? currentActivityType = null;
            if (Context.CurrentActivity!.IsActivityAlive())
                currentActivityType = Context.CurrentActivity!.GetType();

            if (currentActivityType == null)
                return null;

            Singleton<ViewViewModelsKeyContainerManager>.Instance.TryGetValue(currentActivityType, out var viewModelType);
            return viewModelType;
        }

        protected virtual async void ShowHostActivity(FragmentPresentationAttribute attribute)
        {
            if (attribute.ActivityHostViewModelType == null)
                throw new ArgumentException("ActivityHostViewModelType not set on attribute");

            var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance
                .GetValue(attribute.ActivityHostViewModelType);

            if (viewType?.IsSubclassOf(typeof(Activity)) != true)
                throw new AppException("The host activity doesn't inherit Activity");

            var hostViewModelRequest = ViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            if (PendingRequest != null)
                hostViewModelRequest.PresentationValues = PendingRequest.PresentationValues;

            await Show(hostViewModelRequest);
        }

        public virtual void OnFragmentChanged(FragmentTransaction? fragmentTransaction, Fragment? fragment, FragmentPresentationAttribute? attribute, ViewModelRequest? request)
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
