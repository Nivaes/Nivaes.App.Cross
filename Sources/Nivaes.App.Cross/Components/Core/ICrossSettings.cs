namespace Nivaes.App.Cross
{
    public interface ICrossSettings
    {
        bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

        bool ShouldRaisePropertyChanging { get; set; }
    }
}
