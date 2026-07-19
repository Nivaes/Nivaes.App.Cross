namespace Nivaes.App.Cross
{
    internal static class RequestCache
    {
        private struct Entry(nint id, ICrossViewModel value)
        {
            public nint Id = id;
            public ICrossViewModel Value = value;
        }

        private static Lock _lock = new Lock();
        private static Entry[] _entries = [];
        private static int idSequence = 0;

        public static bool TryGetValue(nint id, out ICrossViewModel? value)
        {
            lock (_lock)
            {
                var entries = _entries;

                for (int i = 0; i < entries.Length; i++)
                {
                    ref readonly var entry = ref entries[i];

                    if (entry.Id == id)
                    {
                        value = entry.Value;

                        Array.Copy(entries, i + 1, entries, i, entries.Length - i - 1);
                        Array.Resize(ref _entries, entries.Length - 1);

                        return true;
                    }
                }

                value = null;
                return false;
            }
        }

        public static int Add(ICrossViewModel value)
        {
            lock (_lock)
            {
                int id = idSequence++;
                var entries = _entries;

                Array.Resize(ref _entries, entries.Length + 1);
                _entries[^1] = new Entry(id, value);

                return id;
            }
        }
    }
}
