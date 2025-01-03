using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nivaes.App.Cross.Presenters;

namespace Nivaes.App.Cross
{
    internal class KeyContainerManager
    {
        private KeyPresentation[] mValues { get; }

        public struct KeyPresentation
        {
            public int Key { get; set; }
            public Type Value { get; set; }
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
