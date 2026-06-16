using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    using MvvmCross;

    [Obsolete("", true)]
    [Preserve(AllMembers = true)]
    public class MvxViewBackgroundColorBinding
        : MvxViewColorBinding
    {
        public MvxViewBackgroundColorBinding(View view)
            : base(view)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = (View)target;
            view?.SetBackgroundColor((global::Android.Graphics.Color)value);
        }
    }
}
