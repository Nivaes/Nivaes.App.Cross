namespace Nivaes.App.Cross
{
    public sealed class CrashInfo
    {
        public DateTime Timestamp { get; set; }

        public string Type { get; set; } = "";

        public string Message { get; set; } = "";

        public string StackTrace { get; set; } = "";
    }
}
