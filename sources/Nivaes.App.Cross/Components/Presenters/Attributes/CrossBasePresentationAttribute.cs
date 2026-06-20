namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class CrossBasePresentationAttribute 
        : Attribute, ICrossPresentationAttribute
    {
        /// <inheritdoc />
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? ViewModelType { [DebuggerHidden]get; [DebuggerHidden]set; }

        /// <inheritdoc />
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? ViewType { [DebuggerHidden]get; [DebuggerHidden]set; }
    }
}
