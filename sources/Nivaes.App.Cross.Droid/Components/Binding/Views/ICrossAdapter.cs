namespace Nivaes.App.Cross.Droid
{
    using System.Collections;

    public interface ICrossAdapter
        : ISpinnerAdapter
        , IListAdapter
    {
        [CrossSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }
        int DropDownItemTemplateId { get; set; }

        object GetRawItem(int position);

        int GetPosition(object value);
    }
}
