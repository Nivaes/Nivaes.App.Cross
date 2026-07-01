namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public static class CrossInteractionExtensions
    {
        extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] T>(T interaction)
            where T : ICrossInteraction
        {
            [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Runtime event inspection for generic type parameter with PublicEvents annotation")]
            public IDisposable? WeakSubscribe(EventHandler<EventArgs> action)
            {
                var eventInfo = interaction.GetType().GetEvent("Requested");
                return eventInfo?.WeakSubscribe(interaction, action);
            }
        }

        extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TInteraction, TValue>(TInteraction interaction)
            where TInteraction : ICrossInteraction<TValue>
        {
            [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Runtime event inspection for generic type parameter with PublicEvents annotation")]
            public CrossValueEventSubscription<TValue>? WeakSubscribe(
            EventHandler<CrossValueEventArgs<TValue>> action)
            {
                var eventInfo = interaction.GetType().GetEvent("Requested");
                return eventInfo?.WeakSubscribe(interaction, action);
            }

            public CrossValueEventSubscription<TValue>? WeakSubscribe(Action<TValue> action)
            {
                EventHandler<CrossValueEventArgs<TValue>> wrappedAction = (sender, args) => action(args.Value);
                return interaction.WeakSubscribe(wrappedAction);
            }
        }
    }
}
