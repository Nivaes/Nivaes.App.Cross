namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class CrossTargetBindingFactoryRegistryExtensions
    {
        public static void RegisterCustomBindingFactory<TView>(
            this ICrossTargetBindingFactoryRegistry registry,
            string customName,
            Func<TView, ICrossTargetBinding> creator)
            where TView : class
        {
            registry.RegisterFactory(new CrossCustomBindingFactory<TView>(customName, creator));
        }

        [RequiresUnreferencedCode("This method creates bindings using reflection which may not be preserved by trimming")]
        public static void RegisterPropertyInfoBindingFactory(
            this ICrossTargetBindingFactoryRegistry registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type bindingType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type targetType,
            string targetName)
        {
            registry.RegisterFactory(new CrossSimplePropertyInfoTargetBindingFactory(bindingType, targetType, targetName));
        }
    }
}
