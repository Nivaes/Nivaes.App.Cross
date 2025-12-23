namespace Nivaes.App.Cross.Droid
{
    using System.Collections;
    using System.Windows.Input;
    using MvvmCross.DroidX.RecyclerView.ItemTemplates;

    public interface IMvxRecyclerAdapter
    {
        [CrossSetToNullAfterBinding]
        IEnumerable? ItemsSource { get; set; }

        IMvxTemplateSelector? ItemTemplateSelector { get; set; }
        ICommand? ItemClick { get; set; }
        ICommand? ItemLongClick { get; set; }

        object? GetItem(int viewPosition);

        int ItemTemplateId { get; set; }
    }
}