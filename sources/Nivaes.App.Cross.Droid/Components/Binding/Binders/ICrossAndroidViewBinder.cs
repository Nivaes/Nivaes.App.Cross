namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content;
    using Android.Util;
    using Android.Views;

    public interface ICrossAndroidViewBinder
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        void BindView(View view, Context context, IAttributeSet attrs);

        IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings { get; }
    }
}
