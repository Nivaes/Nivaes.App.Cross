using System;
using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross
{
    public interface ICrossSourceStep
        : ICrossBinding
    {
        Type? TargetType { get; set; }
        Type SourceType { get; }

        void SetValue(object? value);

        event EventHandler Changed;

        object? GetValue();

        object? DataContext
        {
            get;
            [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
            set;
        }
    }
}
