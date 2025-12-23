namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public abstract class CrossViewModel
        : CrossNotifyPropertyChanged, ICrossViewModel
    {
        protected CrossViewModel()
        {
        }

        public virtual void ViewCreated()
        {
        }

        public virtual void ViewAppearing()
        {
        }

        public virtual void ViewAppeared()
        {
        }

        public virtual void ViewDisappearing()
        {
        }

        public virtual void ViewDisappeared()
        {
        }

        public virtual void ViewDestroy(bool viewFinishing = true)
        {
        }

        public void Init(ICrossBundle parameters)
        {
            InitFromBundle(parameters);
        }

        public void ReloadState(ICrossBundle state)
        {
            ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public void SaveState(ICrossBundle state)
        {
            SaveStateToBundle(state);
        }

        protected virtual void InitFromBundle(ICrossBundle parameters)
        {
        }

        protected virtual void ReloadFromBundle(ICrossBundle state)
        {
        }

        protected virtual void SaveStateToBundle(ICrossBundle bundle)
        {
        }

        public virtual void Prepare()
        {
        }

        public virtual Task Initialize()
        {
            return Task.FromResult(true);
        }

        private CrossNotifyTask? _initializeTask;
        public CrossNotifyTask? InitializeTask
        {
            get => _initializeTask;
            set => SetProperty(ref _initializeTask, value);
        }
    }

    public abstract class CrossViewModel<TParameter> : CrossViewModel, ICrossViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
