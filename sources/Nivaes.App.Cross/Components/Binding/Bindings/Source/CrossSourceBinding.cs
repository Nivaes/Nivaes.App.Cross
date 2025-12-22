namespace Nivaes.App.Cross
{
    using System;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Binding.Bindings.Source;

    public abstract class CrossSourceBinding
        : MvxBinding, ICrossSourceBinding
    {
        private readonly object _source;

        protected CrossSourceBinding(object source)
        {
            _source = source;
        }

        protected object Source => _source;

        public event EventHandler Changed;

        public abstract void SetValue(object value);

        public abstract Type SourceType { get; }

        public abstract object GetValue();

        protected void FireChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        protected bool EqualsCurrentValue(object testValue)
        {
            var existing = GetValue();

            if (testValue == null)
            {
                if (existing == null)
                    return true;

                return false;
            }

            return testValue.Equals(existing);
        }
    }
}
