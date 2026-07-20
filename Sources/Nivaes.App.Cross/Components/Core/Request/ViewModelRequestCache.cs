namespace Nivaes.App.Cross
{
    internal static class ViewModelRequestCache
    {
        private struct Entry(uint id, ICrossViewModel value)
        {
            public uint Id = id;
            public ICrossViewModel Value = value;
        }

        private static Lock _lock = new Lock();
        private static Entry[] _entries = [];
        private static uint idSequence = 0;

        public static bool TryGetValue(uint id, out ICrossViewModel? value)
        {
            lock (_lock)
            {
                var entries = _entries;

                for (nint i = entries.Length - 1; i >= 0; i--)
                {
                    ref readonly var entry = ref entries[i];

                    if (entry.Id == id)
                    {
                        value = entry.Value;
                        return true;
                    }
                }

                value = null;
                return false;
            }
        }

        public static bool TryGetValue(ICrossViewModel value, out uint? id)
        {
            lock (_lock)
            {
                var entries = _entries;

                for (nint i = entries.Length - 1; i >= 0; i--)
                {
                    ref readonly var entry = ref entries[i];

                    if (ReferenceEquals(entry.Value, value))
                    {
                        id = entry.Id;
                        return true;
                    }
                }

                id = null;
                return false;
            }
        }

        public static bool TryGetValue(Type viewModelType, out (uint id, ICrossViewModel viewModel)? returnValue)
        {
            lock (_lock)
            {
                var entries = _entries;

                for (nint i = entries.Length - 1; i >= 0; i--)
                {
                    ref readonly var entry = ref entries[i];

                    if (ReferenceEquals(entry.Value.GetType(), viewModelType))
                    {
                        returnValue = (entry.Id, entry.Value);
                        return true;
                    }
                }

                returnValue = null;
                return false;
            }
        }

        public static uint Add(ICrossViewModel value)
        {
            lock (_lock)
            {
                var id = idSequence++;
                var entries = _entries;

                Array.Resize(ref _entries, entries.Length + 1);
                _entries[^1] = new Entry(id, value);

                return id;
            }
        }

        public static bool Delete(uint id)
        {
            lock (_lock)
            {
                var entries = _entries;

                for (nint i = entries.Length - 1; i >= 0; i--)
                {
                    ref readonly var entry = ref entries[i];

                    if (entry.Id == id)
                    {
                        Array.Copy(entries, i + 1, entries, i, entries.Length - i - 1);
                        Array.Resize(ref _entries, entries.Length - 1);

                        return true;
                    }
                }

                return false;
            }
        }
    }
}
