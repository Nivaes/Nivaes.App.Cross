using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public class CrossBindingRequest
    {
        public CrossBindingRequest()
        {
        }

        public CrossBindingRequest(object? source, object? target, CrossBindingDescription? description)
        {
            Target = target;
            Source = source;
            Description = description;
        }

        public object? Target { [DebuggerHidden] get; [DebuggerHidden] set; }
        public object? Source { [DebuggerHidden] get; [DebuggerHidden] set; }
        public CrossBindingDescription? Description { [DebuggerHidden] get; [DebuggerHidden] set; }
    }
}
