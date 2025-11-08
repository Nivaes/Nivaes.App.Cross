namespace Nivaes.App.Cross.Droid
{
    public interface ICrossSavedStateConverter
    {
        ICrossBundle Read(Bundle bundle);

        void Write(Bundle bundle, ICrossBundle savedState);
    }
}
