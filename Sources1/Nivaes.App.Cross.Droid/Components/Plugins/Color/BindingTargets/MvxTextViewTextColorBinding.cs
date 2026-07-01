namespace Nivaes.App.Cross.Droid
{
    public class MvxTextViewTextColorBinding
        : MvxViewColorBinding
    {
        public MvxTextViewTextColorBinding(TextView textView)
            : base(textView)
        {
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var textView = (TextView)target;
            textView?.SetTextColor((global::Android.Graphics.Color)value!);
        }
    }
}
