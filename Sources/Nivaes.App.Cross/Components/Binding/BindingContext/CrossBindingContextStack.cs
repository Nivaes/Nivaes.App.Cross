namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public class CrossBindingContextStack<TContext>
        : Stack<TContext>, ICrossBindingContextStack<TContext>
    {
        public TContext? Current
        {
            get
            {
                if (Count == 0)
                    return default(TContext);
                return Peek();
            }
        }
    }
}
