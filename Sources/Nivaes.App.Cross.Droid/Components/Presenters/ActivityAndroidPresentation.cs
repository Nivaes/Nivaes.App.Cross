using Android.Content;
using Android.OS;
using Android.Util;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;


namespace Nivaes.App.Cross.Droid
{
    public sealed class ActivityAndroidPresentation
        : AndroidPressenterAction<MvxActivityPresentationAttribute>
    {
        //public const string SharedElementsBundleKey = "__sharedElementsKey";
        private readonly IMvxAndroidViewModelRequestTranslator ViewModelRequestTranslator;

        public ActivityAndroidPresentation(
                ICrossViewsContainer viewsContainer,
                IMvxAndroidCurrentTopActivity androidCurrentTopActivity,
                IMvxAndroidViewModelRequestTranslator viewModelRequestTranslator,
                ILogger<ActivityAndroidPresentation> logger)
            : base(viewsContainer, androidCurrentTopActivity, logger)
        {
            ViewModelRequestTranslator = viewModelRequestTranslator;
        }

        // ToDo: Poner ICrossPresentationAttribute como generico.
        protected override ValueTask<bool> ShowAction(Type viewType, MvxActivityPresentationAttribute attribute, CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewType);
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(request);

            var intent = CreateIntentForRequest(request);
            if (intent == null)
                return ValueTask.FromResult(false);

            if (attribute.Extras != null)
                intent.PutExtras(attribute.Extras);

            ShowIntent(intent, CreateActivityTransitionOptions(intent, attribute, request));
            return ValueTask.FromResult(true);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, MvxActivityPresentationAttribute attribute)
        {
            var currentView = CurrentActivity as ICrossView;

            if (currentView == null)
            {
                Logger.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe has no current page");
                return ValueTask.FromResult(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                Logger.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return ValueTask.FromResult(false);
            }

            // don't kill the dead
            if (CurrentActivity.IsActivityAlive())
                CurrentActivity!.Finish();

            return ValueTask.FromResult(true);
        }

        private Bundle CreateActivityTransitionOptions(
           Intent intent, MvxActivityPresentationAttribute attribute, CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(request);


            ArgumentNullException.ThrowIfNull(intent, nameof(intent));

            var bundle = Bundle.Empty!;

            if (!(CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity))
            {
                return bundle;
            }

            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                Logger.Log(LogLevel.Warning, "Shared element transition requires Android v21+");
                return bundle;
            }

            if (CurrentActivity.IsActivityAlive())
            {
                var (elements, transitionElementPairs) =
                    GetTransitionElements(attribute, request, sharedElementsActivity);

                if (transitionElementPairs.Count == 0)
                {
                    Logger.Log(LogLevel.Warning, "No transition elements are provided");
                    return bundle;
                }

                var transitionElementsBundle = CreateTransitionElementsBundle(intent, transitionElementPairs, elements);
                if (transitionElementsBundle != null)
                    return transitionElementsBundle;
            }

            return bundle;
        }

        private Bundle? CreateTransitionElementsBundle(
            Intent intent, IEnumerable<Pair> transitionElementPairs, IEnumerable<string> elements)
        {
            var activityOptions = ActivityOptions.MakeSceneTransitionAnimation(
                CurrentActivity, transitionElementPairs.ToArray());
            if (activityOptions == null)
                return null;

            intent.PutExtra(AndroidViewPresenterManager.SharedElementsBundleKey, string.Join("|", elements));
            var activityOptionsBundle = activityOptions.ToBundle();
            return activityOptionsBundle;
        }

        private void ShowIntent(Intent intent, Bundle? bundle)
        {
            ArgumentNullException.ThrowIfNull(intent, nameof(intent));

            var activity = CurrentActivity;
            if (activity!.IsActivityDead())
            {
                Logger.Log(LogLevel.Error, "Cannot Resolve current top activity. Creating new activity from Application Context");
                intent.AddFlags(ActivityFlags.NewTask);
                StartActivity(Application.Context, intent, bundle);
                return;
            }

            StartActivity(activity!, intent, bundle);
        }

        private void StartActivity(Context context, Intent intent, Bundle? bundle)
        {
            if (bundle != null)
            {
                context.StartActivity(intent, bundle);
            }
            else
            {
                context.StartActivity(intent);
            }
        }

        private Intent? CreateIntentForRequest(CrossViewModelRequest? request)
        {
            if (request is CrossViewModelInstanceRequest viewModelInstanceRequest)
            {
                var intentWithKey = ViewModelRequestTranslator.GetIntentWithKeyFor(
                    viewModelInstanceRequest.ViewModelInstance!,
                    viewModelInstanceRequest
                );

                return intentWithKey.intent;
            }

            return ViewModelRequestTranslator.GetIntentFor(request!);
        }

        private (List<string> elements, List<Pair> transitionElementPairs) GetTransitionElements(
            CrossBasePresentationAttribute attribute, CrossViewModelRequest request,
            IMvxAndroidSharedElements sharedElementsActivity)
        {
            var elements = new List<string>();
            var transitionElementPairs = new List<Pair>();

            foreach (var (key, value) in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
            {
                var transitionName = value.GetTransitionNameSupport();
                if (!string.IsNullOrEmpty(transitionName))
                {
                    var pair = Pair.Create(value, transitionName);
                    if (pair != null)
                    {
                        transitionElementPairs.Add(pair);
                        elements.Add($"{key}:{transitionName}");
                    }
                }
                else
                {
                    Logger.Log(LogLevel.Warning, "A XML transitionName is required in order to transition a control when navigating");
                }
            }

            return (elements, transitionElementPairs);
        }

    }
}
