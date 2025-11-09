namespace Nivaes.App.Cross
{
    public interface ICrossViewModelRequest
    {
        ICrossViewModel ViewModel { get; }

        Type? ViewModelType { get; }
    }
}
