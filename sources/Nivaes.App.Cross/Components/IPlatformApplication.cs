namespace Nivaes.App.Cross
{
    public interface IPlatformApplication
    {
        public static IPlatformApplication? Current { get; set; }

        public IServiceProvider Services { get; }

        public IApplication Application { get; }
    }
}
