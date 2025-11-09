namespace Nivaes.App.Cross.Droid
{
    public class CrossJavaContainer : Java.Lang.Object
    {
        protected CrossJavaContainer(object? theObject)
        {
            Object = theObject;
        }

        public object? Object { get; private set; }
    }

    public class MvxJavaContainer<T> : CrossJavaContainer
    {
        public MvxJavaContainer(T theObject)
            : base(theObject)
        {
        }

        public new T? Object => (T?)base.Object;
    }
}
