using Android.Content;
using Android.Util;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public interface IMvxAndroidViewBinder
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        void BindView(View view, Context context, IAttributeSet attrs);

        IList<KeyValuePair<object, ICrossUpdateableBinding>> CreatedBindings { get; }
    }
}
