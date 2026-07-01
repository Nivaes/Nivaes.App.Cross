namespace Nivaes.App.Cross.Droid
{
    using System.Collections.Generic;
    using Android.Content;
    using Android.Util;
    using Android.Views;

    public interface IMvxLayoutInflaterHolderFactory
        : IMvxLayoutInflaterFactory
    {
        IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings { get; }

        View BindCreatedView(View view, Context? context, IAttributeSet? attrs);
    }
}
