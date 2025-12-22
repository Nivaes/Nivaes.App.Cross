namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public class CrossIndexerChainedSourceBinding
        : CrossChainedSourceBinding
    {
        private readonly MvxIndexerPropertyToken _indexerPropertyToken;

        public CrossIndexerChainedSourceBinding(object source, PropertyInfo itemPropertyInfo, MvxIndexerPropertyToken indexerPropertyToken,
                                                  IList<IMvxPropertyToken> childTokens)
            : base(source, itemPropertyInfo, childTokens)
        {
            _indexerPropertyToken = indexerPropertyToken;
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return new[] { _indexerPropertyToken.Key };
        }
    }
}
