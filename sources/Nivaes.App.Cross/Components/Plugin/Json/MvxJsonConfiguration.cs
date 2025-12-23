namespace Nivaes.App.Cross
{
    using MvvmCross;

    [Preserve(AllMembers = true)]
    public class MvxJsonConfiguration
        : IMvxPluginConfiguration
    {
        public static readonly MvxJsonConfiguration Default = new();

        public MvxJsonConfiguration()
        {
            RegisterAsTextSerializer = true;
        }

        public bool RegisterAsTextSerializer { get; set; }
    }
}
