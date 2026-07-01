namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public static class CrossNotifyPropertyChangedExtensions
    {
        private static bool RaiseIfChanging<TReturn>(TReturn backingField, TReturn newValue,
            Func<bool> raiseAction)
        {
            if (EqualityComparer<TReturn>.Default.Equals(backingField, newValue))
                return false;

            return raiseAction();
        }

        public static bool RaiseIfChanging<TSource, TReturn>(this TSource source,
            TReturn backingField,
            TReturn newValue, Expression<Func<TReturn>> propertySelector)
            where TSource : ICrossNotifyPropertyChanged
        {
            return RaiseIfChanging(backingField, newValue, () => source.RaisePropertyChanging(newValue, propertySelector));
        }

        public static bool RaiseIfChanging<TSource, TReturn>(this TSource source,
            TReturn backingField,
            TReturn newValue, [CallerMemberName] string propertyName = "")
            where TSource : ICrossNotifyPropertyChanged
        {
            return RaiseIfChanging(backingField, newValue, () => source.RaisePropertyChanging(newValue, propertyName));
        }

        public static bool RaiseIfChanging<TSource, TReturn>(this TSource source,
            TReturn backingField,
            TReturn newValue, CrossPropertyChangingEventArgs<TReturn> args)
            where TSource : ICrossNotifyPropertyChanged
        {
            return RaiseIfChanging(backingField, newValue, () => source.RaisePropertyChanging(args));
        }

        private static TReturn RaiseAndSetIfChanged<TReturn, TActionParameter>(
            ref TReturn backingField, TReturn newValue,
            Func<TActionParameter, Task> raiseAction,
            TActionParameter raiseActionParameter)
        {
            if (!EqualityComparer<TReturn>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                raiseAction(raiseActionParameter);
            }

            return newValue;
        }

        public static TReturn RaiseAndSetIfChanged<TSource, TReturn>(this TSource source,
            ref TReturn backingField,
            TReturn newValue, Expression<Func<TReturn>> propertySelector)
            where TSource : ICrossNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(ref backingField, newValue, source.RaisePropertyChanged,
                propertySelector);
        }

        public static TReturn RaiseAndSetIfChanged<TSource, TReturn>(this TSource source,
            ref TReturn backingField,
            TReturn newValue, [CallerMemberName] string propertyName = "")
            where TSource : ICrossNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(ref backingField, newValue, source.RaisePropertyChanged,
                propertyName);
        }

        public static TReturn RaiseAndSetIfChanged<TSource, TReturn>(this TSource source,
            ref TReturn backingField,
            TReturn newValue, PropertyChangedEventArgs args)
            where TSource : ICrossNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(ref backingField, newValue, source.RaisePropertyChanged,
                args);
        }
    }
}