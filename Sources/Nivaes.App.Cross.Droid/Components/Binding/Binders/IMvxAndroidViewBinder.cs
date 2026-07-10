using Android.Content;
using Android.Util;
using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxAndroidViewBinder
    {
        void BindView(View view, Context context, IAttributeSet? attrs);

        IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings { get; }
    }
}
