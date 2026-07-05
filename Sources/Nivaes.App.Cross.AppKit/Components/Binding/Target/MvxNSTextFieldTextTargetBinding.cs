using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public class MvxNSTextFieldTextTargetBinding
        : MvxPropertyInfoTargetBinding<NSTextField>
    {
        public MvxNSTextFieldTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var editText = View;
            if (editText == null)
            {
                CrossBindingLogger.GetLogger<MvxNSTextFieldTextTargetBinding>()
                    .LogError($"{nameof(NSTextView)} is null in {nameof(MvxNSTextFieldTextTargetBinding)}");
            }
            else
            {
                editText.Changed += HandleEditTextChanged;
            }
        }

        private void HandleEditTextChanged(object? sender, EventArgs e)
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
