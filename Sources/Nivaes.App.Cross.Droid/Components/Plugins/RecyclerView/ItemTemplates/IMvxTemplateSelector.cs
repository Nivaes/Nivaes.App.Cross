namespace Nivaes.App.Cross.Droid.RecyclerView;

public interface IMvxTemplateSelector
{
    int ItemTemplateId { get; set; }
    int GetItemViewType(object? forItemObject);

    int GetItemLayoutId(int fromViewType);
}
