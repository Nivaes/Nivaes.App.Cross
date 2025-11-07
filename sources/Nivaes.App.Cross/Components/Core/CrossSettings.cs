namespace Nivaes.App.Cross
{
    public class CrossSettings : 
        ICrossSettings
    {
        public bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

        public bool ShouldRaisePropertyChanging { get; set; }

        public bool ShouldLogInpc { get; set; }

        public CrossSettings()
        {
            AlwaysRaiseInpcOnUserInterfaceThread = true;
            ShouldRaisePropertyChanging = true;
            ShouldLogInpc = false;
        }
    }
}
