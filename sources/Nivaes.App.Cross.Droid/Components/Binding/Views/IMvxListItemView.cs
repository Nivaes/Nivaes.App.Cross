using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxListItemView
        : ICrossDataConsumer
    {
        int TemplateId { get; }
        View Content { get; set; }
    }
}
