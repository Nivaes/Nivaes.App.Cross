namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DialogFragmentPresentationAttribute 
        : FragmentPresentationAttribute
    {
        public DialogFragmentPresentationAttribute()
        {
        }

        public DialogFragmentPresentationAttribute(
            bool cancelable = true,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? activityHostViewModelType = null,
            bool addToBackStack = false,
            int enterAnimation = int.MinValue,
            int exitAnimation = int.MinValue,
            int popEnterAnimation = int.MinValue,
            int popExitAnimation = int.MinValue,
            int transitionStyle = int.MinValue,
            bool isCacheableFragment = false)
            : base(
                  activityHostViewModelType,
                  int.MinValue,
                  addToBackStack,
                  enterAnimation,
                  exitAnimation,
                  popEnterAnimation,
                  popExitAnimation,
                  transitionStyle,
                  null,
                  isCacheableFragment)
        {
            Cancelable = cancelable;
        }

        public DialogFragmentPresentationAttribute(
            bool cancelable = true,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? activityHostViewModelType = null,
            bool addToBackStack = false,
            string? enterAnimation = null,
            string? exitAnimation = null,
            string? popEnterAnimation = null,
            string? popExitAnimation = null,
            string? transitionStyle = null,
            bool isCacheableFragment = false)
            : base(
                  activityHostViewModelType,
                  null,
                  addToBackStack,
                  enterAnimation,
                  exitAnimation,
                  popEnterAnimation,
                  popExitAnimation,
                  transitionStyle,
                  null,
                  isCacheableFragment)
        {
            Cancelable = cancelable;
        }

        /// <summary>
        /// Indicates if the dialog can be canceled
        /// </summary>
        public bool Cancelable { get; set; } = true;
    }
}