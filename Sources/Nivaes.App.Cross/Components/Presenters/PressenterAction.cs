namespace Nivaes.App.Cross
{
    public abstract class PressenterAction
    {
        public abstract Task<bool> ShowActon();

        public abstract Task<bool> CloseActon();
    }
}
