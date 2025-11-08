namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;
    using MvvmCross.IoC;

    [Obsolete("No compatible con AoT")]
    public class CrossWindowsBindingBuilder : CrossBindingBuilder
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

        public CrossWindowsBindingBuilder(
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
                    return new CrossWindowsTargetBindingFactoryRegistry();

                default:
                    throw new InvalidOperationException($"Unable to create target binding registry for BindingType: {_bindingType}");
            }
        }

        private void InitializeBindingCreator()
        {
            throw new NotImplementedException();
            //var creator = CreateBindingCreator();
            //Cross.IoCProvider.RegisterSingleton(creator);
        }

        protected virtual ICrossBindingCreator CreateBindingCreator()
        {
            switch (_bindingType)
            {
                case BindingType.Windows:
                    return new CrossWindowsBindingCreator();

                case BindingType.MvvmCross:
                    return new CrossMvvmCrossBindingCreator();

                default:
                    throw new InvalidOperationException($"Unable to create binding creator for BindingType: {_bindingType}");
            }
        }

        protected override void FillDefaultBindingNames(ICrossBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);
            _fillBindingNames?.Invoke(registry);
        }

        protected override void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            if (CrossSingleton<ICrossWindowsAssemblyCache>.Instance != null)
            {
                foreach (var assembly in CrossSingleton<ICrossWindowsAssemblyCache>.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }

            _fillValueConverters?.Invoke(registry);
        }

        protected override void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            base.FillValueCombiners(registry);

            if (CrossSingleton<ICrossWindowsAssemblyCache>.Instance != null)
            {
                foreach (var assembly in CrossSingleton<ICrossWindowsAssemblyCache>.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }

            _fillValueCombiners?.Invoke(registry);
        }

        protected override void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<FrameworkElement>(
                CrossWindowsPropertyBinding.FrameworkElement_Visible,
                view => new CrossVisibleTargetBinding(view));

            registry.RegisterCustomBindingFactory<FrameworkElement>(
                CrossWindowsPropertyBinding.FrameworkElement_Collapsed,
                view => new CrossCollapsedTargetBinding(view));

            registry.RegisterCustomBindingFactory<FrameworkElement>(
                CrossWindowsPropertyBinding.FrameworkElement_Hidden,
                view => new CrossCollapsedTargetBinding(view));

            base.FillTargetFactories(registry);

            _fillTargetFactories?.Invoke(registry);
        }
    }
}
