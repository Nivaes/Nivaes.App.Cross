namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossViewModelRequest
    {
        ICrossViewModel ViewModel { get; }

        Type? ViewModelType { get; }
    }
}
