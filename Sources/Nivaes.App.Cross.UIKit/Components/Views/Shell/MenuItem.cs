namespace Nivaes.App.Cross.UIKitLib
{
    public class MenuItem
    {
        public string Label { get; private set; }

        public string ImageName { get; private set; }

        public ICrossAsyncCommand Command { get; private set; }

        public MenuItem(string label, string imageName, ICrossAsyncCommand command)
        {
            Label = label;
            ImageName = imageName;
            Command = command;
        }
    }
}
