namespace Nivaes.App.Cross
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CrossSetToNullAfterBindingAttribute : 
        Attribute
    {
    }
}
