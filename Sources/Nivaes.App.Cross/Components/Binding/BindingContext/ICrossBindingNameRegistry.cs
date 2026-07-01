namespace Nivaes.App.Cross
{
    using System;
    using System.Linq.Expressions;

    public interface ICrossBindingNameRegistry
    {
        void AddOrOverwrite(Type type, string name);

        void AddOrOverwrite<T>(Expression<Func<T, object>> nameExpression);
    }
}
