namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public interface ICrossListItemView
        : ICrossDataConsumer
    {
        int TemplateId { get; }
        View Content { get; set; }
    }
}
