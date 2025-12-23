namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public abstract class CrossChainedSourceBinding
        : CrossPropertyInfoSourceBinding
    {
        private readonly IList<IMvxPropertyToken> _childTokens;
        private ICrossSourceBinding _currentChildBinding;

        protected CrossChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<IMvxPropertyToken> childTokens)
            : base(source, propertyInfo)
        {
            _childTokens = childTokens;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing && _currentChildBinding != null)
            {
                _currentChildBinding.Changed -= ChildSourceBindingChanged;
                _currentChildBinding.Dispose();
                _currentChildBinding = null;
            }

            base.Dispose(isDisposing);
        }

        private ICrossSourceBindingFactory SourceBindingFactory => MvxBindingSingletonCache.Instance.SourceBindingFactory;

        public override Type SourceType
        {
            get
            {
                if (_currentChildBinding == null)
                    return typeof(object);

                return _currentChildBinding.SourceType;
            }
        }

        [RequiresUnreferencedCode("This method may use types that are not preserved by trimming")]
        protected void UpdateChildBinding()
        {
            if (_currentChildBinding != null)
            {
                _currentChildBinding.Changed -= ChildSourceBindingChanged;
                _currentChildBinding.Dispose();
                _currentChildBinding = null;
            }

            if (PropertyInfo == null)
            {
                return;
            }

            var currentValue = PropertyInfo.GetValue(Source, PropertyIndexParameters());
            if (currentValue == null)
            {
                // value will be missing... so end consumer will need to use fallback values
            }
            else
            {
                _currentChildBinding = SourceBindingFactory.CreateBinding(currentValue, _childTokens);
                _currentChildBinding.Changed += ChildSourceBindingChanged;
            }
        }

        protected abstract object[] PropertyIndexParameters();

        private void ChildSourceBindingChanged(object sender, EventArgs e)
        {
            FireChanged();
        }

        [RequiresUnreferencedCode("This method may use types that are not preserved by trimming")]
        protected override void OnBoundPropertyChanged()
        {
            UpdateChildBinding();
            FireChanged();
        }

        public override object GetValue()
        {
            if (_currentChildBinding == null)
            {
                return MvxBindingConstant.UnsetValue;
            }

            return _currentChildBinding.GetValue();
        }

        public override void SetValue(object value)
        {
            if (_currentChildBinding == null)
            {
                MvxBindingLog.Instance?.LogWarning("SetValue ignored in binding - target property path missing");
                return;
            }

            _currentChildBinding.SetValue(value);
        }
    }
}
