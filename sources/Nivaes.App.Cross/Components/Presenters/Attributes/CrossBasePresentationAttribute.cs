namespace Nivaes.App.Cross
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class CrossBasePresentationAttribute : 
        Attribute, 
        ICrossPresentationAttribute
    {
        /// <inheritdoc />
        public Type? ViewModelType { get; set; }

        /// <inheritdoc />
        public Type? ViewType { get; set; }
    }
}
