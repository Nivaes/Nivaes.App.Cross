namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    public record CrossViewModelRequest<TViewModel> :
        ICrossViewModelRequest
        where TViewModel : ICrossViewModel
    {
        public Type ViewModelType => typeof(TViewModel);
        public IDictionary<string, string>? ParameterValues { get; set; }
        public IDictionary<string, string>? PresentationValues { get; set; }


        public CrossViewModelRequest(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public CrossViewModelRequest(ICrossBundle? parameterBundle, ICrossBundle? presentationBundle)
        {
            ParameterValues = parameterBundle?.Data;
            PresentationValues = presentationBundle?.Data;
        }

        public CrossViewModelRequest(TViewModel viewModel, ICrossBundle? parameterBundle, ICrossBundle? presentationBundle)
            :this(viewModel)
        {
            ParameterValues = parameterBundle?.Data;
            PresentationValues = presentationBundle?.Data;
        }

        public TViewModel ViewModel { get;}
        public CrossNotifyTask InitializeTask { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ICrossViewModel ICrossViewModelRequest.ViewModel => ViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        //public static CrossViewModelRequest<TViewModel> GetDefaultRequest(Type viewModelType)
        //{
        //    return new CrossViewModelRequest<TViewModel>(null, null);
        //}
    }
}
