using System.Collections;
using Android.Widget;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxAdapter
        : ISpinnerAdapter
        , IListAdapter
    {
        [CrossSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }
        int DropDownItemTemplateId { get; set; }

        object? GetRawItem(int position);

        int GetPosition(object value);
    }
}
