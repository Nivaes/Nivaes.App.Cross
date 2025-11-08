namespace Nivaes.App.Cross.Droid
{
    using Android.OS;
    using Android.Views;

    public class CrossCreateViewParameters
    {
        public CrossCreateViewParameters(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            SavedInstanceState = savedInstanceState;
            Container = container;
            Inflater = inflater;
        }

        public LayoutInflater Inflater { get; private set; }
        public ViewGroup? Container { get; private set; }
        public Bundle? SavedInstanceState { get; private set; }
    }
}
