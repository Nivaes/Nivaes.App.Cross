namespace Nivaes.App.Cross
{
    public interface ICrossUpdateableBinding 
        : ICrossBinding
    {
        object? DataContext { get; set; }
    }
}
