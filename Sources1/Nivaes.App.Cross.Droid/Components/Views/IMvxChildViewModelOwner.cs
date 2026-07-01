namespace Nivaes.App.Cross.Droid
{
    using System.Collections.Generic;

    public interface IMvxChildViewModelOwner
    {
        List<int> OwnedSubViewModelIndicies { get; }
    }
}
