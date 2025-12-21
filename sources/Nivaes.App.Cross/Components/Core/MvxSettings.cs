namespace MvvmCross.Core
{
    using Nivaes.App.Cross;

    public class MvxSettings 
        : ICrossSettings
    {
        public bool AlwaysRaiseInpcOnUserInterfaceThread { get; set; }

        public bool ShouldRaisePropertyChanging { get; set; }

        public bool ShouldLogInpc { get; set; }

        public MvxSettings()
        {
            AlwaysRaiseInpcOnUserInterfaceThread = true;
            ShouldRaisePropertyChanging = true;
            ShouldLogInpc = false;
        }
    }
}