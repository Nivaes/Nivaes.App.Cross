using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Nivaes.App.Cross
{
    public class CrossIndexerLeafPropertyInfoSourceBinding 
        : CrossLeafPropertyInfoSourceBinding
    {
        private readonly object? _key;

        public CrossIndexerLeafPropertyInfoSourceBinding(
            object source,
            PropertyInfo itemPropertyInfo,
            CrossIndexerPropertyToken indexToken)
                : base(source, itemPropertyInfo)
        {
            _key = indexToken.Key;
        }

        protected override object[] PropertyIndexParameters()
        {
            return [_key!];
        }
    }
}
