namespace Nivaes.App.Cross
{
    public interface ICrossSourceBinding
        : ICrossBinding
    {
        Type SourceType { get; }

        void SetValue(object? value);

        event EventHandler Changed;

        object? GetValue();
    }
}
