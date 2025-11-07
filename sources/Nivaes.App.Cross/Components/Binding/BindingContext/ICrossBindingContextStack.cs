namespace Nivaes.App.Cross
{
    public interface ICrossBindingContextStack<TContext>
    {
        TContext Current { get; }

        void Push(TContext context);

        TContext Pop();
    }
}
