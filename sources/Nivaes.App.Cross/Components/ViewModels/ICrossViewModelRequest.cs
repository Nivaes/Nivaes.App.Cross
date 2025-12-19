namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossViewModelRequest
    {
        ICrossViewModel ViewModel { get; }

        Type? ViewModelType { get; }

        IDictionary<string, string>? ParameterValues { get; set; }
        IDictionary<string, string>? PresentationValues { get; set; }
    }
}
