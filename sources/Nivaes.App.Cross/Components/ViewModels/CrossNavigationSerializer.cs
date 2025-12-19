namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    [Obsolete]
    public class CrossNavigationSerializer
        : ICrossNavigationSerializer
    {
        public CrossNavigationSerializer(ICrossTextSerializer serializer)
        {
            Serializer = serializer;
        }

        public ICrossTextSerializer Serializer { get; }
    }

    public class MvxNavigationSerializer<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>
            : CrossNavigationSerializer
                where T : class, ICrossTextSerializer
    {
        public MvxNavigationSerializer()
            : base(null/*Mvx.IoCProvider.Resolve<T>()*/)
        {
            throw new NotImplementedException();
        }
    }
}
