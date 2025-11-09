namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public class CrossSimpleLayoutInflaterHolder : ICrossLayoutInflaterHolder
    {
        public CrossSimpleLayoutInflaterHolder(LayoutInflater layoutInflater)
        {
            LayoutInflater = layoutInflater;
        }

        public LayoutInflater LayoutInflater { get; private set; }
    }
}
