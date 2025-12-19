namespace Nivaes.App.Cross
{
    [Obsolete("Quitar IoC de Cross")]
    public interface ICrossSettings
    {
        bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

        bool ShouldRaisePropertyChanging { get; set; }

        bool ShouldLogInpc { get; set; }
    }
}
