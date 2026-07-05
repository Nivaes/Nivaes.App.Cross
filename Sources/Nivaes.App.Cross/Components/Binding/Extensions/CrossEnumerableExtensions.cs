using System.Collections;

namespace Nivaes.App.Cross
{
    public static class CrossEnumerableExtensions
    {
        public static int Count(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return 0;

            var itemsList = enumerable as ICollection;
            if (itemsList != null)
            {
                return itemsList.Count;
            }

            var enumerator = enumerable.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
            {
                count++;
            }

            return count;
        }

        public static int GetPosition(this IEnumerable items, object item)
        {
            if (items == null)
            {
                return -1;
            }

            var itemsList = items as IList;
            if (itemsList != null)
            {
                return itemsList.IndexOf(item);
            }

            var enumerator = items.GetEnumerator();
            for (var i = 0; ; i++)
            {
                if (!enumerator.MoveNext())
                {
                    return -1;
                }

                if (enumerator.Current == null)
                {
                    if (item == null)
                        return i;
                }
                // Note: do *not* use == here - see https://github.com/slodge/MvvmCross/issues/309
                else if (enumerator.Current.Equals(item))
                {
                    return i;
                }
            }
        }

        public static IEnumerable Filter(this IEnumerable items, Func<object, bool> predicate)
        {
            if (items == null)
                return Array.Empty<object>();

            var matchList = new List<object>();
            foreach (var item in items)
            {
                var match = predicate(item);
                if (match)
                    matchList.Add(item);
            }

            if (matchList.Count == 0)
                return Array.Empty<object>();

            return matchList;
        }
    }
}
