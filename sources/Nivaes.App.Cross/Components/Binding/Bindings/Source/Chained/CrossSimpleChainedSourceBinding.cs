namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public class CrossSimpleChainedSourceBinding
        : CrossChainedSourceBinding
    {
        public CrossSimpleChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<ICrossPropertyToken> childTokens)
            : base(source, propertyInfo, childTokens)
        {
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return [];
        }
    }
}
