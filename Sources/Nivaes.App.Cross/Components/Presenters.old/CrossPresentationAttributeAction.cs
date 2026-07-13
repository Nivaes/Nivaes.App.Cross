using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public class CrossPresentationAttributeAction
    {
        public Func<Type, ICrossPresentationAttribute, CrossViewModelRequest, Task<bool>>? ShowAction { [DebuggerHidden] get; [DebuggerHidden] set; }

        public Func<ICrossViewModel, ICrossPresentationAttribute, Task<bool>>? CloseAction { [DebuggerHidden] get; [DebuggerHidden] set; }
    }
}
