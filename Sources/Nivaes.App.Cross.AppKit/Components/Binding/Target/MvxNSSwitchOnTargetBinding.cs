using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public class MvxNSSwitchOnTargetBinding
        : MvxPropertyInfoTargetBinding<NSButton>
    {
        public MvxNSSwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkBox = View;
            if (checkBox == null)
            {
                CrossBindingLogger.GetLogger<MvxNSSwitchOnTargetBinding>().
                    LogError($"{nameof(NSTextView)} is null in {nameof(MvxNSSwitchOnTargetBinding)}");
            }
            else
            {
                checkBox.Activated += HandleButtonCheckBoxAction;
            }
        }

        private void HandleButtonCheckBoxAction(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.State == NSCellStateValue.On);
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        protected override object MakeSafeValue(object value)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    return (NSCellStateValue.On);
                }
                else
                {
                    return (NSCellStateValue.Off);
                }
            }
            return base.MakeSafeValue(value);
        }

        public override CrossBindingMode DefaultMode
        {
            get { return CrossBindingMode.TwoWay; }
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null)
                {
                    view.Activated -= HandleButtonCheckBoxAction;
                }
            }
        }
    }
}
