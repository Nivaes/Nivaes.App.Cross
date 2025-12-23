namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public class MvxSimpleLayoutInflaterHolder
        : IMvxLayoutInflaterHolder
    {
        public MvxSimpleLayoutInflaterHolder(LayoutInflater layoutInflater)
        {
            LayoutInflater = layoutInflater;
        }

        public LayoutInflater LayoutInflater { get; private set; }
    }
}
