namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossSourceBinding 
        : ICrossBinding
    {
        Type SourceType { get; }

        void SetValue(object value);

        event EventHandler Changed;

        object GetValue();
    }
}
