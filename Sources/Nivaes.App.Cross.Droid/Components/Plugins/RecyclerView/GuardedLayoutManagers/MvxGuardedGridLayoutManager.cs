using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;
using Java.Lang;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross.Droid.RecyclerView
{
    [Register("nivaes.cross.GuardedGridLayoutManager")]
    public class MvxGuardedGridLayoutManager : GridLayoutManager
    {
        public MvxGuardedGridLayoutManager(Context context, int spanCount) : base(context, spanCount)
        {
        }

        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MvxGuardedGridLayoutManager))]
        protected MvxGuardedGridLayoutManager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        /// <summary>
        /// Fix issue like https://code.google.com/p/android/issues/detail?id=77846#c1 but may not be exactly the same.
        /// https://stackoverflow.com/questions/30220771/recyclerview-inconsistency-detected-invalid-item-position?page=1&tab=active#tab-top
        /// </summary>
        public override bool SupportsPredictiveItemAnimations() => false;

        /// <summary>
        /// This should not be needed anymore, as it should be caused by SupportsPredictiveItemAnimations.
        /// </summary>
        public override void OnLayoutChildren(
            AndroidX.RecyclerView.Widget.RecyclerView.Recycler? recycler,
            AndroidX.RecyclerView.Widget.RecyclerView.State? state)
        {
            try
            {
                base.OnLayoutChildren(recycler, state);
            }
            catch (IndexOutOfBoundsException e)
            {
                CrossLoggerHost.GetLogger<MvxGuardedGridLayoutManager>().LogWarning(e,
                    "Workaround of issue - https://code.google.com/p/android/issues/detail?id=77846#c1 - IndexOutOfBoundsException");
            }
        }
    }
}