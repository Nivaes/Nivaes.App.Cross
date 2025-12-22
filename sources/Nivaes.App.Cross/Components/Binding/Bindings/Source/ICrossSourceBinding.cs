namespace Nivaes.App.Cross
{
    using System;
    using MvvmCross.Binding.Bindings;

    public interface ICrossSourceBinding 
        : ICrossBinding
    {
        Type SourceType { get; }

        void SetValue(object value);

        event EventHandler Changed;

        object GetValue();
    }
}
