namespace Nivaes.App.Cross
{
    public interface IShellService
    {
        ValueTask InitializeShell();

        ValueTask ShowMenu();
    }
}
