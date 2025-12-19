namespace Nivaes.App.Cross.Droid
{
    [Obsolete()]
    public class CrossJavaContainer : Java.Lang.Object
    {
        protected CrossJavaContainer(object? theObject)
        {
            Object = theObject;
        }

        public object? Object { get; private set; }
    }

    [Obsolete()]
    public class MvxJavaContainer<T> : CrossJavaContainer
    {
        public MvxJavaContainer(T theObject)
            : base(theObject)
        {
        }

        public new T? Object => (T?)base.Object;
    }
}
