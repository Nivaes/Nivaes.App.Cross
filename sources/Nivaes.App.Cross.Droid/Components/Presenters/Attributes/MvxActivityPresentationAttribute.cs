namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
    using Nivaes.App.Cross;

    [AttributeUsage(AttributeTargets.Class)]
    public class MvxActivityPresentationAttribute : CrossBasePresentationAttribute
    {
        public MvxActivityPresentationAttribute()
        {
        }

        public static Bundle? DefaultExtras { get; }

        /// <summary>
        /// Add extras to the Intent that will be started for this Activity
        /// </summary>
        public Bundle? Extras { get; set; } = DefaultExtras;
    }
}