namespace MvvmCross.IoC
{
    using System;

    [Obsolete("Quitar IoC de Cross")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MvxUnconventionalAttribute : Attribute
    {
    }
}
