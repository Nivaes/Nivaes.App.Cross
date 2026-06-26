using System.Diagnostics;

namespace Nivaes.App.Cross;

public class CrossSerializableBindingDescription
{
    public string? Converter { [DebuggerHidden] get; [DebuggerHidden] set; }

    public object? ConverterParameter { [DebuggerHidden] get; [DebuggerHidden] set; }

    public object? FallbackValue { [DebuggerHidden] get; [DebuggerHidden] set; }

    public CrossBindingMode Mode { [DebuggerHidden] get; [DebuggerHidden] set; }

    public IList<CrossSerializableBindingDescription>? Sources { [DebuggerHidden] get; [DebuggerHidden] set; }

    public string? Function { [DebuggerHidden] get; [DebuggerHidden] set; }

    public object? Literal { [DebuggerHidden] get; [DebuggerHidden] set; }

    public string? Path { [DebuggerHidden] get; [DebuggerHidden] set; }
}
