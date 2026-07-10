using Android.Content;
using Android.Util;
using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxLayoutInflaterHolderFactory
        : IMvxLayoutInflaterFactory
    {
        IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings { get; }

        View BindCreatedView(View view, Context context, IAttributeSet? attrs);
    }
}
