namespace Nivaes.App.Cross.Droid
{
    using System.Collections.Generic;

    public interface IMvxChildViewModelOwner
    {
        List<uint> OwnedSubViewModelIndicies { get; }
    }
}
