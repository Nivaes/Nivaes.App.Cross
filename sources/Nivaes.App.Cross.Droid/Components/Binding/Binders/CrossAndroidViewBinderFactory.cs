namespace Nivaes.App.Cross.Droid
{
    public class CrossAndroidViewBinderFactory
        : ICrossAndroidViewBinderFactory
    {
        public ICrossAndroidViewBinder Create(object source)
        {
            return new CrossAndroidViewBinder(source);
        }
    }
}
