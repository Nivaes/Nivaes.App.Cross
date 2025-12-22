namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming")]
    public class CrossIndexerLeafPropertyInfoSourceBinding : CrossLeafPropertyInfoSourceBinding
    {
        private readonly object _key;

        public CrossIndexerLeafPropertyInfoSourceBinding(
            object source,
            PropertyInfo itemPropertyInfo,
            MvxIndexerPropertyToken indexToken)
                : base(source, itemPropertyInfo)
        {
            _key = indexToken.Key;
        }

        protected override object[] PropertyIndexParameters()
        {
            return [_key];
        }
    }
}
