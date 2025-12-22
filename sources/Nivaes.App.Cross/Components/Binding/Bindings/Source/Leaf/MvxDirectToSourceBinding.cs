namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    [RequiresUnreferencedCode("This class uses GetType() on source objects which may not be preserved by trimming")]
    public class MvxDirectToSourceBinding : CrossSourceBinding
    {
        public MvxDirectToSourceBinding(object source)
            : base(source)
        {
        }

        public override Type SourceType => Source == null ? typeof(object) : Source.GetType();

        public override void SetValue(object value)
        {
            MvxBindingLog.Instance?.LogWarning("ToSource binding is not available for direct pathed source bindings");
        }

        public override object GetValue()
        {
            return Source;
        }
    }
}
