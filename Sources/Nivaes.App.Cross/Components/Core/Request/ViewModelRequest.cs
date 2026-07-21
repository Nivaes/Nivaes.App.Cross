using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public interface IViewModelRequest
    { }

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

        private Lazy<ICrossViewModel> _viewModel;

        protected virtual ICrossViewModel LoadViewModel()
        {
            var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
            return viewModelLoader.LoadViewModel(ViewModelType, ParameterValues, null, null);
        }

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

        public override string ToString()
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

    public record ViewModelRequest<TViewModel>
        : ViewModelRequest
        where TViewModel : ICrossViewModel
    {
        public ViewModelRequest()
            : base(typeof(TViewModel))
        {
        }

        public ViewModelRequest(TViewModel viewModel) 
            : base(viewModel)
        {
        }

        public ViewModelRequest(ICrossBundle? parameterBundle, ICrossBundle? presentationBundle)
            : base(typeof(TViewModel), parameterBundle, presentationBundle)
        {
        }
    }

    public record ViewModelRequest<TViewModel, TParameter>
         : ViewModelRequest<TViewModel>
         where TViewModel : ICrossViewModel<TParameter>
         where TParameter : notnull
    {
        public TParameter Parameter { get; set; }

        public ViewModelRequest(TParameter parameter)
        {
            Parameter = parameter;
        }

        public ViewModelRequest(TViewModel viewModel, TParameter parameter)
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
}