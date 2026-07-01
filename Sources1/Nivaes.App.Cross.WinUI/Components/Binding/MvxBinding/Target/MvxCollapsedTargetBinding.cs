namespace Nivaes.App.Cross.WinUI
{
    public class MvxCollapsedTargetBinding : MvxVisibleTargetBinding
    {
        public MvxCollapsedTargetBinding(object target)
            : base(target)
        {
        }

        public override void SetValue(object value)
        {
            if (value == null)
                value = false;
            var boolValue = (bool)value;
            base.SetValue(!boolValue);
        }
    }
}
