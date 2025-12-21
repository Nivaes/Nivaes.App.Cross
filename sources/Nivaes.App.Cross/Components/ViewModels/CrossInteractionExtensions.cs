namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Base;
    using MvvmCross.WeakSubscription;

    public static class CrossInteractionExtensions
    {
        [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Runtime event inspection for generic type parameter with PublicEvents annotation")]
        public static IDisposable? WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] T>(
            this T interaction, EventHandler<EventArgs> action)
                where T : ICrossInteraction
        {
            var eventInfo = interaction.GetType().GetEvent("Requested");
            return eventInfo?.WeakSubscribe(interaction, action);
        }

        [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Runtime event inspection for generic type parameter with PublicEvents annotation")]
        public static MvxValueEventSubscription<TValue>? WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TInteraction, TValue>(
            this TInteraction interaction,
            EventHandler<MvxValueEventArgs<TValue>> action)
                where TInteraction : ICrossInteraction<TValue>
        {
            var eventInfo = interaction.GetType().GetEvent("Requested");
            return eventInfo?.WeakSubscribe(interaction, action);
        }

        public static MvxValueEventSubscription<TValue>? WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TInteraction, TValue>(
            this TInteraction interaction, Action<TValue> action)
                where TInteraction : ICrossInteraction<TValue>
        {
            EventHandler<MvxValueEventArgs<TValue>> wrappedAction = (sender, args) => action(args.Value);
            return interaction.WeakSubscribe(wrappedAction);
        }
    }
}
