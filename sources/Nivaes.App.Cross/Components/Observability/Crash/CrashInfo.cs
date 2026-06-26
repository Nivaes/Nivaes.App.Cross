namespace Nivaes.App.Cross.Observability;

public sealed class CrashInfo
{
    public DateTime Timestamp { get; set; }

    public string? Type { get; set; }

    public string? Message { get; set; }

    public string? StackTrace { get; set; }

    public string? Source { get; set; }

    public IDictionary<string, object?>? Data { get; set; }

    public CrashInfo? InnerException { get; set; }

    public CrashInfo(Exception ex)
    {
        Timestamp = DateTime.UtcNow;
        Type = ex.GetType().FullName ?? "";
        Message = ex.Message;
        StackTrace = ex.ToString();
        Source = ex.Source;
        Data = ex.Data.Count > 0
                        ? ex.Data.Keys.Cast<object>()
                                    .ToDictionary(k => k.ToString()!, k => ex.Data[k])
                        : null;
        InnerException = ex.InnerException != null ? new CrashInfo(ex.InnerException) : null;
    }
}
