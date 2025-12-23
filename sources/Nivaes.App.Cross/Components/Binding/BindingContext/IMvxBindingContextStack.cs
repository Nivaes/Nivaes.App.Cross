namespace Nivaes.App.Cross
{
    public interface IMvxBindingContextStack<TContext>
    {
        TContext? Current { get; }

        void Push(TContext context);

        TContext Pop();
    }
}
