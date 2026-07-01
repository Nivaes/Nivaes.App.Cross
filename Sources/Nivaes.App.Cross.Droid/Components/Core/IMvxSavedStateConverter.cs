namespace Nivaes.App.Cross.Droid
{
    public interface IMvxSavedStateConverter
    {
        ICrossBundle? Read(Bundle? bundle);

        void Write(Bundle bundle, ICrossBundle? savedState);
    }
}
