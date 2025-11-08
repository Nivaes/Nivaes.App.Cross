namespace Nivaes.App.Cross.WinUI3
{
    public class CrossCollapsedTargetBinding : CrossVisibleTargetBinding
    {
        public CrossCollapsedTargetBinding(object target)
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
