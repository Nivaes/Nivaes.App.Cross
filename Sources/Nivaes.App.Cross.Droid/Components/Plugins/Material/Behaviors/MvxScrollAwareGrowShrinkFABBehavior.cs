using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.View;
using Google.Android.Material.FloatingActionButton;
using Object = Java.Lang.Object;

namespace Nivaes.App.Cross.Droid.Material
{
    [Register("nivaes.cross.material.ScrollAwareGrowShrinkFABBehavior")]
    public class MvxScrollAwareGrowShrinkFABBehavior
        : CoordinatorLayout.Behavior
    {
        public MvxScrollAwareGrowShrinkFABBehavior(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public MvxScrollAwareGrowShrinkFABBehavior()
        {
        }

        public MvxScrollAwareGrowShrinkFABBehavior(Context context, IAttributeSet attributeSet)
            : base(context, attributeSet)
        {
        }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View directTargetChild, View target, int axes, int type)
        {
            return axes == ViewCompat.ScrollAxisVertical ||
                base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target, axes, type);
        }

        public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int type)
        {
            base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, type);

            var floatingActionButtonChild = child.JavaCast<FloatingActionButton>();

            if (dyConsumed > 0 && floatingActionButtonChild.Visibility == ViewStates.Visible)
                floatingActionButtonChild.Hide();
            else if (dyConsumed < 0 && floatingActionButtonChild.Visibility != ViewStates.Visible)
                floatingActionButtonChild.Show();
        }
    }
}
