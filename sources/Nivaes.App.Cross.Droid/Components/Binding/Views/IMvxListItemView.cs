using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Views
{   
    using MvvmCross.Base;
    using Nivaes.App.Cross;

    public interface IMvxListItemView
        : ICrossDataConsumer
    {
        int TemplateId { get; }
        View Content { get; set; }
    }
}
