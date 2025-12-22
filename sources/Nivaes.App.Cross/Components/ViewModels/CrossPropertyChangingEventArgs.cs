namespace Nivaes.App.Cross
{
    using System.ComponentModel;

    public class CrossPropertyChangingEventArgs<T> 
        : PropertyChangingEventArgs
    {
        public CrossPropertyChangingEventArgs(string propertyName, T newValue) 
            : base(propertyName)
        {
            NewValue = newValue;
        }

        public bool Cancel { get; set; }

        public T NewValue { get; set; }
    }
}
