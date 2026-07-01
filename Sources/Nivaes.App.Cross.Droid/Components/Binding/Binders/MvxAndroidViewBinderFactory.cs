namespace Nivaes.App.Cross.Droid
{
    public class MvxAndroidViewBinderFactory
        : IMvxAndroidViewBinderFactory
    {
        public IMvxAndroidViewBinder Create(object source)
        {
            return new MvxAndroidViewBinder(source);
        }
    }
}
