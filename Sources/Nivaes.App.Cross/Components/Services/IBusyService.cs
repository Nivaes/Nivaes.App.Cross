namespace Nivaes.App.Cross;

public interface IBusyService
{
    ValueTask Show(Func<Task> action);

    ValueTask<T> Show<T>(Func<Task<T>> action);

    ValueTask Show(string pregressText, Func<Task> action);

    ValueTask<T> Show<T>(string pregressText, Func<Task<T>> action);
}
