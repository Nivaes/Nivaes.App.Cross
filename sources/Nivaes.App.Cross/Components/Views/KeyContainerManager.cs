namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    public class KeyContainerManager<TValue>
    {
        private KeyPresentation[] mValues;

        public struct KeyPresentation
        {
            public int Key { get; set; }
            public TValue Value { get; set; }
        }

        private class KeyPresentationComparer : IComparer<KeyPresentation>
        {
            public int Compare(KeyPresentation x, KeyPresentation y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }

        public KeyContainerManager()
        {
            mValues = new KeyPresentation[0];
        }

        public KeyContainerManager(KeyPresentation[] values)
        {
            mValues = values;
            var keyInstanceResolverValues = new Span<KeyPresentation>(mValues);
            keyInstanceResolverValues.Sort(new KeyPresentationComparer());
        }

        public void Merge(KeyPresentation[] newValues)
        {
            var oldValues = mValues;
            var allValues = new KeyPresentation[oldValues.Length + newValues.Length];
            int i = 0, j = 0, m = 0;

            while (i < oldValues.Length && j < newValues.Length)
            {
                if (oldValues[i].Key < newValues[j].Key)
                {
                    allValues[m++] = oldValues[i++];
                }
                else
                {
                    allValues[m++] = newValues[j++];
                }
            }
            while (i < oldValues.Length)
            {
                allValues[m++] = oldValues[i++];
            }
            while (j < newValues.Length)
            {
                allValues[m++] = newValues[j++];
            }

            mValues = allValues;
        }

        public bool TryGetValue<TView>([MaybeNullWhen(false)] out TValue presentationType)
           where TView : IView
        {
            return TryGetValue(typeof(TView), out presentationType);
        }

        public bool TryGetValue(Type viewType, [MaybeNullWhen(false)] out TValue presentationType)
        {
            var result = TryGetValue(viewType.GetHashCode(), out int position);

            if (result)
            {
                presentationType = mValues[position].Value;
                return true;
            }
            else
            {
                presentationType = default;
                return false;
            }
        }

        protected bool TryGetValue(int key, [MaybeNullWhen(false)] out int position)
        {
            var high = mValues.Length - 1;
            var low = 0;

            while (low <= high)
            {
                int mid = (high + low) / 2;
                var midKey = mValues[mid].Key;

                if (midKey == key)
                {
                    position = mid;
                    return true;
                }
                else
                {
                    if (key < midKey)
                        high = mid - 1;
                    else
                        low = mid + 1;
                }
            }
            position = -1;
            return false;
        }
    }
}
