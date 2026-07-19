using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public class ViewModelRequest
    {
        [Obsolete]
        public ViewModelRequest()
        {
        }

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
            set;
        }

        private Lazy<ICrossViewModel> _viewModel;

        private ICrossViewModel LoadViewModel()
        {
            var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();
            return viewModelLoader.LoadViewModel(ViewModelType, null, null);
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
            sb.Append($"MvxViewModelRequest - ViewModelType: '{ViewModelType}'");
            if (ParameterValues != null)
            {
                sb.Append(
                    $", ParameterValues: '{string.Join(", ", ParameterValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
            }

            if (PresentationValues != null)
            {
                sb.Append(
                    $", PresentationValues: '{string.Join(", ", PresentationValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
            }

            return sb.ToString();
        }
    }

    public class ViewModelRequest<TViewModel>
        : ViewModelRequest 
        where TViewModel : ICrossViewModel
    {
        public ViewModelRequest(TViewModel viewModel) 
            : base(viewModel)
        {
        }

        public ViewModelRequest(ICrossBundle? parameterBundle, ICrossBundle? presentationBundle)
            : base(typeof(TViewModel), parameterBundle, presentationBundle)
        {
        }

        public static ViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof(TViewModel));
        }
    }
}