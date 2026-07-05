namespace Nivaes.App.Cross.Droid
{
    public class MvxJavaContainer : Java.Lang.Object
    {
        protected MvxJavaContainer(object theObject)
        {
            Object = theObject;
        }

        public object Object { get; private set; }
    }

    public class MvxJavaContainer<T> : MvxJavaContainer
    {
        public MvxJavaContainer(T theObject)
            : base(theObject)
        {
        }

        public new T Object => (T)base.Object;
    }
}
