namespace Nivaes.App.Cross.Droid.RecyclerView;

public class MvxDefaultTemplateSelector : IMvxTemplateSelector
{
    public int ItemTemplateId { get; set; }

    public MvxDefaultTemplateSelector(int itemTemplateId)
    {
        ItemTemplateId = itemTemplateId;
    }

    public MvxDefaultTemplateSelector()
    {
    }

    public int GetItemViewType(object? forItemObject)
        => 0;

    public int GetItemLayoutId(int fromViewType)
        => ItemTemplateId;
}
