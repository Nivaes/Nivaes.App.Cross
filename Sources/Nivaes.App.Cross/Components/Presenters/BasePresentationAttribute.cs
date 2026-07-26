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
        [Obsolete("Usar PresentationData.ViewModelType")]
        public Type? ViewModelType { [DebuggerHidden] get; [DebuggerHidden] set; }

        /// <inheritdoc />
        [Obsolete("Usar PresentationData.ViewType")]
        public Type? ViewType { [DebuggerHidden] get; [DebuggerHidden] set; }
    }
}
