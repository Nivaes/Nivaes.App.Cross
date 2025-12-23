namespace Nivaes.App.Cross.Droid
{
    using System.Collections;

    public interface IMvxObjectAdapter
    {
        [CrossSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }
    }
}
