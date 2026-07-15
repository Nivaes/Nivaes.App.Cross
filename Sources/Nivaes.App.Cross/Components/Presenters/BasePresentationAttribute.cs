using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BasePresentationAttribute
        : Attribute, IPresentationAttribute
    {
        /// <inheritdoc />
        public Type? ViewModelType { [DebuggerHidden] get; [DebuggerHidden] set; }

        /// <inheritdoc />
        public Type? ViewType { [DebuggerHidden] get; [DebuggerHidden] set; }
    }
}
