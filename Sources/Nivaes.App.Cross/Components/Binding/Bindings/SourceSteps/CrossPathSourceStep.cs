namespace Nivaes.App.Cross
{
    public class CrossPathSourceStep
        : CrossSourceStep<CrossPathSourceStepDescription>
    {
        private ICrossSourceBinding? _sourceBinding;

        private readonly Lock _sourceLocker = new ();

        public CrossPathSourceStep(CrossPathSourceStepDescription description)
            : base(description)
        {
        }

        private ICrossSourceBindingFactory SourceBindingFactory => Singleton<CrossBindingSingletonCache>.Instance.SourceBindingFactory;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearPathSourceBinding();
            }

            base.Dispose(isDisposing);
        }

        public override Type SourceType
        {
            get
            {
                if (_sourceBinding == null)
                    return typeof(object);

                return _sourceBinding.SourceType;
            }
        }

        //TODO: optim: dont recreate the source binding on each datacontext change, as SourcePropertyPath does not change.
        //TODO: optim: don't subscribe to the Changed event if the binding mode does not need it.
        protected override void OnDataContextChanged()
        {
            ClearPathSourceBinding();
            _sourceBinding = SourceBindingFactory.CreateBinding(DataContext, Description.SourcePropertyPath);
            if (_sourceBinding != null)
            {
                _sourceBinding.Changed += SourceBindingOnChanged;
            }
            base.OnDataContextChanged();
        }

        private void ClearPathSourceBinding()
        {
            lock (_sourceLocker)
            {
                if (_sourceBinding != null)
                {
                    _sourceBinding.Changed -= SourceBindingOnChanged;
                    _sourceBinding.Dispose();
                }
            }
        }

        private void SourceBindingOnChanged(object? sender, EventArgs args)
        {
            SendSourcePropertyChanged();
        }

        protected override void SetSourceValue(object? sourceValue)
        {
            if (_sourceBinding == null)
                return;

            if (sourceValue == CrossBindingConstant.UnsetValue)
                return;

            if (sourceValue == CrossBindingConstant.DoNothing)
                return;

            _sourceBinding.SetValue(sourceValue);
        }

        protected override object? GetSourceValue()
        {
            if (_sourceBinding == null)
            {
                return CrossBindingConstant.UnsetValue;
            }

            return _sourceBinding.GetValue();
        }
    }
}
