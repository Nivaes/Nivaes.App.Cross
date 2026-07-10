using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross
{
    public interface ICrossBindingContext
        : ICrossDataConsumer
    {
        event EventHandler DataContextChanged;

        ICrossBindingContext Init(object? dataContext, object firstBindingKey, IEnumerable<CrossBindingDescription> firstBindingValue);

        ICrossBindingContext Init(object? dataContext, object firstBindingKey, string firstBindingValue);

        void RegisterBinding(object target, ICrossUpdateableBinding binding);

        void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, ICrossUpdateableBinding>> bindings);

        void RegisterBindingWithClearKey(object clearKey, object target, ICrossUpdateableBinding binding);

        void ClearBindings(object clearKey);

        void ClearAllBindings();

        void DelayBind(Action action);
    }
}
