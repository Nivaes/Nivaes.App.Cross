namespace MvvmCross.Platforms.Mac.Binding.Target
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxNSTextFieldTextTargetBinding 
        : MvxPropertyInfoTargetBinding<NSTextField>
    {
        public MvxNSTextFieldTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var editText = View;
            if (editText == null)
            {
                CrossBindingLogger.Instance?.LogError("NSTextField is null in MvxNSTextFieldTextTargetBinding");
            }
            else
            {
                editText.Changed += HandleEditTextChanged;
            }
        }

        private void HandleEditTextChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.StringValue);
        }

        public override CrossBindingMode DefaultMode
        {
            get { return CrossBindingMode.TwoWay; }
        }

        protected override void SetValueImpl(object target, object value)
        {
            base.SetValueImpl(target, value ?? "");
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
                    editText.Changed -= HandleEditTextChanged;
                }
            }
        }
    }
}
