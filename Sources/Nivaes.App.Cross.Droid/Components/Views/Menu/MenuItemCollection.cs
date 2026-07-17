namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.ObjectModel;

    public class MenuItemCollection : KeyedCollection<int, MenuItem>
    {
        protected override int GetKeyForItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return item.ItemId;
        }
    }
}
