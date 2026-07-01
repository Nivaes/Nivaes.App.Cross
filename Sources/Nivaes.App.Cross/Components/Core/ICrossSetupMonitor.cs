namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface ICrossSetupMonitor
    {
        Task InitializationComplete();
    }
}
