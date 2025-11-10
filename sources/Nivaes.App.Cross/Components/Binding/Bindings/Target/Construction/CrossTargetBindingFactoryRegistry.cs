namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    public class CrossTargetBindingFactoryRegistry : ICrossTargetBindingFactoryRegistry
    {
        private readonly Dictionary<int, ICrossPluginTargetBindingFactory> _lookups = [];

        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method creates bindings using reflection which may not be preserved by trimming")]
        public virtual ICrossTargetBinding CreateBinding(object target, string targetName)
        {
            if (TryCreateSpecificFactoryBinding(target, targetName, out ICrossTargetBinding first))
                return first;

            if (TryCreateReflectionBasedBinding(target, targetName, out ICrossTargetBinding second))
                return second;

            return null;
        }

        [Obsolete("No compatible con AoT")]
        [RequiresUnreferencedCode("This method uses reflection to access properties and events which may not be preserved by trimming")]
        protected virtual bool TryCreateReflectionBasedBinding(
            object target, string targetName, out ICrossTargetBinding binding)
        {
            if (string.IsNullOrEmpty(targetName))
            {
                CrossBindingLog.Instance?.LogError("Empty binding target passed to CrossTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            if (target == null)
            {
                // null passed in so return false - fixes #584
                binding = null;
                return false;
            }

            var targetPropertyInfo = target.GetType().GetProperty(targetName);
            if (targetPropertyInfo != null
                && targetPropertyInfo.CanWrite)
            {
                binding = new CrossWithEventPropertyInfoTargetBinding(target, targetPropertyInfo);
                return true;
            }

            var targetEventInfo = target.GetType().GetEvent(targetName);
            if (targetEventInfo != null && targetEventInfo.EventHandlerType == typeof(EventHandler))
            {
                // we only handle EventHandler's here
                // other event types will need to be handled by custom bindings
                binding = new CrossEventHandlerEventInfoTargetBinding(target, targetEventInfo);
                return true;
            }

            binding = null;
            return false;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected virtual bool TryCreateSpecificFactoryBinding(object target, string targetName,
                                                               out ICrossTargetBinding binding)
        {
            if (target == null)
            {
                // null passed in so return false - fixes #584
                binding = null;
                return false;
            }

            var factory = FindSpecificFactory(target.GetType(), targetName);
            if (factory != null)
            {
                binding = factory.CreateBinding(target, targetName);
                return true;
            }

            binding = null;
            return false;
        }

        public void RegisterFactory(ICrossPluginTargetBindingFactory factory)
        {
            foreach (var supported in factory.SupportedTypes)
            {
                var key = GenerateKey(supported.Type, supported.Name);
                _lookups[key] = factory;
            }
        }

        private static int GenerateKey(Type type, string name)
        {
            return (type.GetHashCode() * 9) ^ name.GetHashCode();
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' requirements",
            Justification = "The interface types returned by ImplementedInterfaces on a type with DynamicallyAccessedMemberTypes.Interfaces are safe to process")]
        private ICrossPluginTargetBindingFactory FindSpecificFactory(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type type, string name)
        {
            ICrossPluginTargetBindingFactory factory;
            var key = GenerateKey(type, name);
            if (_lookups.TryGetValue(key, out factory))
            {
                return factory;
            }
            var baseType = type.GetTypeInfo().BaseType;
            if (baseType != null)
                factory = FindSpecificFactory(baseType, name);
            if (factory != null) return factory;
            var implementedInterfaces = type.GetTypeInfo().ImplementedInterfaces;
            foreach (var implementedInterface in implementedInterfaces)
            {
                factory = FindSpecificFactory(implementedInterface, name);
                if (factory != null) return factory;
            }
            return null;
        }
    }
}
