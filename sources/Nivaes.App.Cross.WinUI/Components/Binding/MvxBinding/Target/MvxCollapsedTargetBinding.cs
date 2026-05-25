namespace Nivaes.App.Cross.WinUI3
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
