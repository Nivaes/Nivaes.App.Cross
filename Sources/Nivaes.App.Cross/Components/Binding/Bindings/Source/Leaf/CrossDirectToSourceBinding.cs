using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public class CrossDirectToSourceBinding 
        : CrossSourceBinding
    {
        public CrossDirectToSourceBinding(object source)
            : base(source)
        {
        }

        public override Type SourceType => Source == null ? typeof(object) : Source.GetType();

        public override void SetValue(object value)
        {
            CrossBindingLogger.GetLogger<CrossDirectToSourceBinding>().LogWarning("ToSource binding is not available for direct pathed source bindings");
        }

        public override object? GetValue()
        {
            return Source;
        }
    }
}
