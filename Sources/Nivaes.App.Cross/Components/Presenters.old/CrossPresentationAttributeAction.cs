using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public class CrossPresentationAttributeAction
    {
        public Func<Type, IPresentationAttribute, CrossViewModelRequest, Task<bool>>? ShowAction { [DebuggerHidden] get; [DebuggerHidden] set; }

        public Func<ICrossViewModel, IPresentationAttribute, Task<bool>>? CloseAction { [DebuggerHidden] get; [DebuggerHidden] set; }
    }
}
