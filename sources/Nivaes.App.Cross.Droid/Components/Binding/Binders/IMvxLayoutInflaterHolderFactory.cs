using Android.Content;
using Android.Util;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
    using System.Collections.Generic;
    using Android.Views;
    using MvvmCross.Binding.Bindings;

    public interface IMvxLayoutInflaterHolderFactory : IMvxLayoutInflaterFactory
    {
        IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings { get; }

        View BindCreatedView(View view, Context context, IAttributeSet attrs);
    }
}
