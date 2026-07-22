using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public interface IViewModelRequest
    {
        Type ViewModelType { get; }

        ICrossViewModel ViewModel { get; }

        IDictionary<string, string>? ParameterValues { get; }

        IDictionary<string, string>? PresentationValues { get; }

        public string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"ViewModelRequest - ViewModelType: '{ViewModelType}'");
            if (ParameterValues != null)
            {
                sb.Append($", ParameterValues: '{string.Join(", ", ParameterValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
            }

            if (PresentationValues != null)
            {
                sb.Append($", PresentationValues: '{string.Join(", ", PresentationValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
            }

            return sb.ToString();
        }
    }

    public record class ViewModelRequest
        : IViewModelRequest
    {
        public ViewModelRequest(Type viewModelType)
        {
            ViewModelType = viewModelType;
            _viewModel = new Lazy<ICrossViewModel>(LoadViewModel);
        }

        public ViewModelRequest(Type viewModelType,
            ICrossBundle? parameterBundle,
            ICrossBundle? presentationBundle)
            :this(viewModelType) 
        {
            ParameterValues = parameterBundle.SafeGetData();
            PresentationValues = presentationBundle.SafeGetData();
        }

        public ViewModelRequest(ICrossViewModel viewModel)
        {
            ViewModelType = viewModel.GetType();
            _viewModel = new Lazy<ICrossViewModel>(viewModel);
        }

        #region ViewModel
        public Type ViewModelType
        {
            get;
        }

        protected virtual ICrossViewModel LoadViewModel()
        {
            var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
            return viewModelLoader.LoadViewModel(ViewModelType, ParameterValues, null, null);
        }

        private Lazy<ICrossViewModel> _viewModel;

        public ICrossViewModel ViewModel
        {
            get => _viewModel.Value;
        }
        #endregion

        public IDictionary<string, string>? ParameterValues 
        { 
            get; 
            set; 
        }

        public IDictionary<string, string>? PresentationValues 
        { 
            get; 
            set; 
        }

        public static ViewModelRequest GetDefaultRequest(Type viewModelType)
        {
            return new ViewModelRequest(viewModelType, null, null);
        }
    }

    public record ViewModelRequest<TViewModel>
        : IViewModelRequest
        where TViewModel : ICrossViewModel
    {
        public ViewModelRequest()
        {
            _viewModel = new Lazy<TViewModel>(LoadViewModel);
        }

        public ViewModelRequest(TViewModel viewModel) 
        {
        }

        public ViewModelRequest(ICrossBundle? parameterBundle, ICrossBundle? presentationBundle)
        {
        }



        #region ViewModel
        public Type ViewModelType => typeof(TViewModel);

        protected virtual TViewModel LoadViewModel()
        {
            var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
            return viewModelLoader.LoadViewModel<TViewModel>(ParameterValues, null, null);
        }

        private Lazy<TViewModel> _viewModel;

        public TViewModel ViewModel
        {
            get => _viewModel.Value;
        }

        ICrossViewModel IViewModelRequest.ViewModel => ViewModel;
        #endregion

        public IDictionary<string, string>? ParameterValues
        {
            get;
            set;
        }

        public IDictionary<string, string>? PresentationValues
        {
            get;
            set;
        }
    }

    public record ViewModelRequestParameter<TParameter>
         : ViewModelRequest
         where TParameter : notnull
    {
        public TParameter Parameter { get; set; }

        public ViewModelRequestParameter(Type viewModelType, TParameter parameter)
            :base(viewModelType)
        {
            Parameter = parameter;
        }

        public ViewModelRequestParameter(ICrossViewModel viewModel, TParameter parameter)
            : base(viewModel)
        {
            Parameter = parameter;
        }

        protected override ICrossViewModel LoadViewModel()
        {
            var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
            return viewModelLoader.LoadViewModel<TParameter>(ViewModelType, ParameterValues, Parameter, null, null);
        }
    }

    public record ViewModelRequestParameter<TViewModel, TParameter>
         : ViewModelRequest<TViewModel>
         where TViewModel : ICrossViewModel<TParameter>
         where TParameter : notnull
    {
        public TParameter Parameter { get; set; }

        public ViewModelRequestParameter(TParameter parameter)
        {
            Parameter = parameter;
        }

        public ViewModelRequestParameter(TViewModel viewModel, TParameter parameter)
            : base(viewModel)
        { 
            Parameter = parameter;
        }

        protected override TViewModel LoadViewModel()
        {
            var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
            return viewModelLoader.LoadViewModel<TViewModel, TParameter>(ParameterValues, Parameter, null, null);
        }
    }
}