namespace Nivaes.App.Cross
{
    using MvvmCross;

    [Preserve(AllMembers = true)]
    public class CrossJsonConfiguration
        : IMvxPluginConfiguration
    {
        public static readonly CrossJsonConfiguration Default = new();

        public CrossJsonConfiguration()
        {
            RegisterAsTextSerializer = true;
        }

        public bool RegisterAsTextSerializer { get; set; }
    }
}
