namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    public class CrossCustomBindingFactory<TTarget>
        : ICrossPluginTargetBindingFactory
        where TTarget : class
    {
        private readonly Func<TTarget, ICrossTargetBinding> _targetBindingCreator;
        private readonly string _targetFakePropertyName;

        public CrossCustomBindingFactory(string targetFakePropertyName,
                                       Func<TTarget, ICrossTargetBinding> targetBindingCreator)
        {
            _targetFakePropertyName = targetFakePropertyName;
            _targetBindingCreator = targetBindingCreator;
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<CrossTypeAndNamePair> SupportedTypes => new[]
        {
            new CrossTypeAndNamePair(typeof(TTarget), _targetFakePropertyName)
        };

        public ICrossTargetBinding? CreateBinding(object target, string targetName)
        {
            if (target is not TTarget castTarget)
            {
                CrossBindingLogger.GetLogger<CrossCustomBindingFactory<TTarget>>().LogError("Passed an invalid target for MvxCustomBindingFactory");
                return null;
            }

            return _targetBindingCreator(castTarget);
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}
