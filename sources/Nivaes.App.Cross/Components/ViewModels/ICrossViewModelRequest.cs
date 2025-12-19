namespace Nivaes.App.Cross
{
    using System;

    [Obsolete]
    public interface ICrossViewModelRequest
    {
        ICrossViewModel ViewModel { get; }

        Type? ViewModelType { get; }
    }
}
