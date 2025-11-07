namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    [Obsolete("No compatible con AoT", true)]
    public class CrossPropertyInfoTargetBindingFactory
        : ICrossPluginTargetBindingFactory
    {
        private readonly Func<object, PropertyInfo, ICrossTargetBinding> _bindingCreator;
        private readonly string _targetName;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
        private readonly Type _targetType;

        public CrossPropertyInfoTargetBindingFactory(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type targetType,
            string targetName,
            Func<object, PropertyInfo, ICrossTargetBinding> bindingCreator)
        {
            _targetType = targetType;
            _targetName = targetName;
            _bindingCreator = bindingCreator;
        }

        protected Type TargetType => _targetType;

        #region ICrossPluginTargetBindingFactory Members

        public IEnumerable<CrossTypeAndNamePair> SupportedTypes => new[]
        {
            new CrossTypeAndNamePair { Name = _targetName, Type = _targetType }
        };

        [RequiresUnreferencedCode("This method uses reflection to get properties which may not be preserved by trimming")]
        public ICrossTargetBinding CreateBinding(object target, string targetName)
        {
            var targetPropertyInfo = target.GetType().GetProperty(targetName);
            if (targetPropertyInfo != null)
            {
                try
                {
                    return _bindingCreator(target, targetPropertyInfo);
                }
                catch (Exception exception)
                {
                    CrossBindingLog.Instance?.LogError(
                        exception,
                        "Problem creating target binding for {TargetName} - exception {ExceptionMessage}", _targetType.Name,
                        exception.ToString());
                }
            }

            return null;
        }

        #endregion ICrossPluginTargetBindingFactory Members
    }
}
