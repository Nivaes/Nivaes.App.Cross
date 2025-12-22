namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUITextViewTextTargetBinding
        : CrossConvertingTargetBinding
    {
        protected UITextView View => Target as UITextView;

        private bool _subscribed;

        public MvxUITextViewTextTargetBinding(UITextView target)
            : base(target)
        {
        }

        private void EditTextOnChanged(object sender, EventArgs eventArgs)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var target = View;
            if (target == null)
            {
                MvxBindingLog.Instance?.LogError("UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

            target.Changed += EditTextOnChanged;
            _subscribed = true;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UITextView)target;
            if (view == null)
                return;

            view.Text = (string)value;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_subscribed)
                {
                    var target = View;
                    if (target != null)
                    {
                        target.Changed -= EditTextOnChanged;
                    }
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
