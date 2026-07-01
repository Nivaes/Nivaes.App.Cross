namespace Nivaes.App.Cross
{
    using System;

    public class CrossMissingSourceBinding
        : CrossSourceBinding
    {
        public CrossMissingSourceBinding(object? source) : base(source)
        {
        }

        public override void SetValue(object value)
        {
            // nothing we can do here - binding is missing
        }

        public override Type SourceType => typeof(object);

        public override object GetValue()
        {
            // binding is missing so return 'unset value'
            return CrossBindingConstant.UnsetValue;
        }
    }
}
