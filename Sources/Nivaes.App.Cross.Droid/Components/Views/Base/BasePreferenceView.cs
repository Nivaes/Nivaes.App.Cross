namespace Nivaes.App.Cross.Droid
{
    public abstract class BasePreferenceView<TViewModel>
        : AppCompatPreferenceActivity //, AppBarLayout.IOnOffsetChangedListener
        where TViewModel : class, ICrossViewModel
    {
        #region Properties
        //private const float cPercentageShowTitleToolbar = 0.6f;
        //private const float cPercentageHideTitleDetails = 0.4f;
        //private const int cAlphaAnimationDuration = 200;

        //private bool mIsTheTitleVisible = false;
        //private bool mIsTheTitleContainerVisible = true;

        //protected androidx.appcompat.widget.Toolbar Toolbar { get; set; }
        //protected LinearLayout TitleContainer { get; set; }
        //protected TextView TitleView { get; set; }
        //protected AppBarLayout AppBarLayout { get; set; }

        //protected abstract int LayoutId { get; }
        #endregion

        //protected override void OnCreate(Bundle bundle)
        //{
        //    base.OnCreate(bundle);

        //    //base.SetContentView(LayoutId);

        //    //BindActivity();
        //}

        //#region Animation
        //private void BindActivity()
        //{
        //    TitleView = base.FindViewById<TextView>(Resource.Id.main_textview_title);
        //    if (TitleView != null)
        //    {
        //        TitleView.Visibility = ViewStates.Gone;
        //        StartAlphaAnimation(TitleView, 0, ViewStates.Invisible);
        //    }

        //    TitleContainer = base.FindViewById<LinearLayout>(Resource.Id.main_linearlayout_title);

        //    AppBarLayout = base.FindViewById<AppBarLayout>(Resource.Id.appbar);
        //    AppBarLayout?.AddOnOffsetChangedListener(this);

        //    Toolbar = FindViewById<androidx.appcompat.widget.Toolbar>(Resource.Id.toolbar);
        //    if (Toolbar != null)
        //    {
        //        base.SetSupportActionBar(Toolbar);
        //        base.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        //        base.SupportActionBar.SetHomeButtonEnabled(true);
        //    }

        //}

        //public static void StartAlphaAnimation(View v, long duration, ViewStates visibility)
        //{
        //    AlphaAnimation alphaAnimation = (visibility == ViewStates.Visible)
        //        ? new AlphaAnimation(0f, 1f)
        //        : new AlphaAnimation(1f, 0f);

        //    alphaAnimation.Duration = duration;
        //    alphaAnimation.FillAfter = true;
        //    v.StartAnimation(alphaAnimation);
        //}

        //void AppBarLayout.IOnOffsetChangedListener.OnOffsetChanged(AppBarLayout appBarLayout, int verticalOffset)
        //{
        //    int maxScroll = appBarLayout.TotalScrollRange;
        //    float percentage = (float)Math.Abs(verticalOffset) / (float)maxScroll;

        //    HandleAlphaOnTitle(percentage);
        //    HandleToolbarTitleVisibility(percentage);
        //}

        //private void HandleToolbarTitleVisibility(float percentage)
        //{
        //    if (percentage >= cPercentageShowTitleToolbar)
        //    {
        //        if (!mIsTheTitleVisible)
        //        {
        //            StartAlphaAnimation(TitleView, cAlphaAnimationDuration, ViewStates.Visible);
        //            TitleView.Visibility = ViewStates.Visible;
        //            mIsTheTitleVisible = true;
        //        }
        //    }
        //    else
        //    {
        //        if (mIsTheTitleVisible)
        //        {
        //            StartAlphaAnimation(TitleView, cAlphaAnimationDuration, ViewStates.Invisible);
        //            TitleView.Visibility = ViewStates.Gone;
        //            mIsTheTitleVisible = false;
        //        }
        //    }
        //}

        //private void HandleAlphaOnTitle(float percentage)
        //{
        //    if (percentage >= cPercentageHideTitleDetails)
        //    {
        //        if (mIsTheTitleContainerVisible)
        //        {
        //            StartAlphaAnimation(TitleContainer, cAlphaAnimationDuration, ViewStates.Invisible);

        //            base.SupportActionBar?.SetDisplayShowTitleEnabled(false);
        //            mIsTheTitleContainerVisible = false;
        //        }
        //    }
        //    else
        //    {
        //        if (!mIsTheTitleContainerVisible)
        //        {
        //            StartAlphaAnimation(TitleContainer, cAlphaAnimationDuration, ViewStates.Visible);

        //            base.SupportActionBar?.SetDisplayShowTitleEnabled(true);
        //            mIsTheTitleContainerVisible = true;
        //        }
        //    }
        //}
        //#endregion

        /// <summary>Closes app if back button is pressed.</summary>
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount > 0)
            {
                FragmentManager.PopBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}
