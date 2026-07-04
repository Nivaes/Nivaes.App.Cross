namespace Nivaes.App.Cross.UIKitLib;

public class NSExceptionWrapper : Exception
{
    public NSException NSException { get; }

    public NSExceptionWrapper(NSException ex)
        : base($"{ex.Name}: {ex.Reason}")
    {
        NSException = ex;
    }
}
