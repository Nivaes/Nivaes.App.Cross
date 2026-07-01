namespace Nivaes.App.Cross
{
    using System;

    public class CrossSourcePropertyBindingEventArgs
        : EventArgs
    {
        private readonly object _value;

        public CrossSourcePropertyBindingEventArgs(object value)
        {
            _value = value;
        }

        public CrossSourcePropertyBindingEventArgs(ICrossSourceBinding propertySourceBinding)
        {
            _value = propertySourceBinding.GetValue();
        }

        public object Value => _value;
    }
}
