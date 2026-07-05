using System.Collections;

namespace Nivaes.App.Cross.Droid.Leanback
{
    public interface IMvxObjectAdapter
    {
        [CrossSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }
    }
}
