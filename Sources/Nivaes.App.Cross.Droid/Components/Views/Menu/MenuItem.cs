namespace Nivaes.App.Cross.Droid
{
    public class MenuItem
    {
        public string Label { get; private set; }

        public int ItemId { get; private set; }

        public ICrossAsyncCommand Command { get; private set; }

        public MenuItem(string label, int itemId, ICrossAsyncCommand command)
        {
            Label = label;
            ItemId = itemId;
            Command = command;
        }
    }
}
