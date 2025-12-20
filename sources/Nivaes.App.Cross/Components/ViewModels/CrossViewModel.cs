namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;

    public abstract class CrossViewModel
        : MvxNotifyPropertyChanged, ICrossViewModel
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

        public void Init(IMvxBundle parameters)
        {
            InitFromBundle(parameters);
        }

        public void ReloadState(IMvxBundle state)
        {
            ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public void SaveState(IMvxBundle state)
        {
            SaveStateToBundle(state);
        }

        protected virtual void InitFromBundle(IMvxBundle parameters)
        {
        }

        protected virtual void ReloadFromBundle(IMvxBundle state)
        {
        }

        protected virtual void SaveStateToBundle(IMvxBundle bundle)
        {
        }

        public virtual void Prepare()
        {
        }

        public virtual Task Initialize()
        {
            return Task.FromResult(true);
        }

        private MvxNotifyTask? _initializeTask;
        public MvxNotifyTask? InitializeTask
        {
            get => _initializeTask;
            set => SetProperty(ref _initializeTask, value);
        }
    }

    public abstract class MvxViewModel<TParameter> : CrossViewModel, ICrossViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
