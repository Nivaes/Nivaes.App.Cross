namespace Nivaes.App.Cross.Droid
{
    using Android.Content;
    using Android.Util;
    using Android.Views;

    public interface ICrossLayoutInflaterHolderFactory : ICrossLayoutInflaterFactory
    {
        IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings { get; }

        View BindCreatedView(View view, Context context, IAttributeSet attrs);
    }
}
