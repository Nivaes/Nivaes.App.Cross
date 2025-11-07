namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossBindingContext
        : ICrossDataConsumer
    {
        event EventHandler DataContextChanged;

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        ICrossBindingContext Init(object dataContext, object firstBindingKey, IEnumerable<CrossBindingDescription> firstBindingValue);

        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming")]
        ICrossBindingContext Init(object dataContext, object firstBindingKey, string firstBindingValue);

        void RegisterBinding(object target, ICrossUpdateableBinding binding);

        void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, ICrossUpdateableBinding>> bindings);

        void RegisterBindingWithClearKey(object clearKey, object target, ICrossUpdateableBinding binding);

        void ClearBindings(object clearKey);

        void ClearAllBindings();

        void DelayBind(Action action);
    }
}
