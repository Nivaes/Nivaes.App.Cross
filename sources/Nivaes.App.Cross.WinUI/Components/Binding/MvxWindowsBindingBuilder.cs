namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.UI.Xaml;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.WinUi.Binding;

    [Obsolete("")]
    public class MvxWindowsBindingBuilder
        : CrossBindingBuilder
    {
        public enum BindingType
        {
            Windows,
            MvvmCross
        }

        private readonly BindingType _bindingType;
        private readonly Action<ICrossTargetBindingFactoryRegistry> _fillTargetFactories;
        private readonly Action<ICrossBindingNameRegistry> _fillBindingNames;
        private readonly Action<ICrossValueConverterRegistry> _fillValueConverters;
        private readonly Action<ICrossValueCombinerRegistry> _fillValueCombiners;

        public MvxWindowsBindingBuilder(
            Action<ICrossTargetBindingFactoryRegistry> fillTargetFactories = null,
            Action<ICrossBindingNameRegistry> fillBindingNames = null,
            Action<ICrossValueConverterRegistry> fillValueConverters = null,
            Action<ICrossValueCombinerRegistry> fillValueCombiners = null,
            BindingType bindingType = BindingType.MvvmCross)
        {
            _fillTargetFactories = fillTargetFactories;
            _fillBindingNames = fillBindingNames;
            _fillValueConverters = fillValueConverters;
            _fillValueCombiners = fillValueCombiners;
            _bindingType = bindingType;
        }

        public override void DoRegistration(IMvxIoCProvider iocProvider)
        {
            base.DoRegistration(iocProvider);
            InitializeBindingCreator();
        }

        protected override void RegisterBindingFactories(IMvxIoCProvider iocProvider)
        {
            switch (_bindingType)
            {
                case BindingType.Windows:
                    // no need for MvvmCross binding factories - so don't create them
                    break;

                case BindingType.MvvmCross:
                    base.RegisterBindingFactories(iocProvider);
                    break;

                default:
                    throw new InvalidOperationException($"Unable to register binding factories for BindingType: {_bindingType}");
            }
        }

        protected override ICrossTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            switch (_bindingType)
            {
                case BindingType.Windows:
                    return base.CreateTargetBindingRegistry();

                case BindingType.MvvmCross:
                    return new MvxWindowsTargetBindingFactoryRegistry();

                default:
                    throw new InvalidOperationException($"Unable to create target binding registry for BindingType: {_bindingType}");
            }
        }

        [Obsolete]
        private void InitializeBindingCreator()
        {
            var creator = CreateBindingCreator();

            //Mvx.IoCProvider.RegisterSingleton(creator);
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddInstance(creator);
        }

        protected virtual IMvxBindingCreator CreateBindingCreator()
        {
            switch (_bindingType)
            {
                case BindingType.Windows:
                    return new MvxWindowsBindingCreator();

                case BindingType.MvvmCross:
                    return new MvxMvvmCrossBindingCreator();

                default:
                    throw new InvalidOperationException($"Unable to create binding creator for BindingType: {_bindingType}");
            }
        }

        protected override void FillDefaultBindingNames(ICrossBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);
            _fillBindingNames?.Invoke(registry);
        }

        [Obsolete("No usar reflection")]
        protected override void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            if (CrossSingleton<IMvxWindowsAssemblyCache>.Instance != null)
            {
                foreach (var assembly in CrossSingleton<IMvxWindowsAssemblyCache>.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }

            _fillValueConverters?.Invoke(registry);
        }

        [Obsolete("No usar reflection")]
        protected override void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            base.FillValueCombiners(registry);

            if (CrossSingleton<IMvxWindowsAssemblyCache>.Instance != null)
            {
                foreach (var assembly in CrossSingleton<IMvxWindowsAssemblyCache>.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }

            _fillValueCombiners?.Invoke(registry);
        }

        protected override void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<FrameworkElement>(
                MvxWindowsPropertyBinding.FrameworkElement_Visible,
                view => new MvxVisibleTargetBinding(view));

            registry.RegisterCustomBindingFactory<FrameworkElement>(
                MvxWindowsPropertyBinding.FrameworkElement_Collapsed,
                view => new MvxCollapsedTargetBinding(view));

            registry.RegisterCustomBindingFactory<FrameworkElement>(
                MvxWindowsPropertyBinding.FrameworkElement_Hidden,
                view => new MvxCollapsedTargetBinding(view));

            base.FillTargetFactories(registry);

            _fillTargetFactories?.Invoke(registry);
        }
    }
}
