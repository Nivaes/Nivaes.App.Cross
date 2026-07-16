using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public class CrossSimplePropertyInfoTargetBindingFactory
        : ICrossPluginTargetBindingFactory
    {
        private readonly Type _bindingType;
        private readonly CrossPropertyInfoTargetBindingFactory _innerFactory;

        public CrossSimplePropertyInfoTargetBindingFactory(Type bindingType, Type targetType,
            string targetName)
        {
            _bindingType = bindingType;
            _innerFactory = new CrossPropertyInfoTargetBindingFactory(targetType, targetName, CreateTargetBinding);
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<CrossTypeAndNamePair> SupportedTypes => _innerFactory.SupportedTypes;

        [RequiresUnreferencedCode("This method creates target bindings using reflection-based binding creation which may not be preserved by trimming")]
        public ICrossTargetBinding CreateBinding(object target, string targetName)
        {
            return _innerFactory.CreateBinding(target, targetName);
        }

        #endregion IMvxPluginTargetBindingFactory Members

        private ICrossTargetBinding? CreateTargetBinding(object target, PropertyInfo targetPropertyInfo)
        {
            var targetBindingCandidate = Activator.CreateInstance(_bindingType, target, targetPropertyInfo);
            var targetBinding = targetBindingCandidate as ICrossTargetBinding;
            if (targetBinding == null)
            {
                CrossBindingLogger.GetLogger<CrossSimplePropertyInfoTargetBindingFactory>().LogWarning("The TargetBinding created did not support IMvxTargetBinding");
                var disposable = targetBindingCandidate as IDisposable;
                disposable?.Dispose();
            }
            return targetBinding;
        }
    }
}
