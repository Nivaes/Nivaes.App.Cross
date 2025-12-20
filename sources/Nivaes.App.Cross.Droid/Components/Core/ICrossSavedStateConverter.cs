namespace Nivaes.App.Cross.Droid
{
    [Obsolete()]
    public interface ICrossSavedStateConverter
    {
        ICrossBundle Read(Bundle bundle);

        void Write(Bundle bundle, ICrossBundle savedState);
    }
}
