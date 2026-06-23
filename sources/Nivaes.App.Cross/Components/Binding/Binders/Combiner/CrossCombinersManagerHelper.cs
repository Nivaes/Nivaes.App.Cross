using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public static class CrossCombinersManagerHelper
    {
        public sealed class CombinersManagerItem
        {
            internal CrossNameCombinerManager.KeyStoreItem NameCombiners { [DebuggerHidden]get; [DebuggerHidden]set; }
            internal CrossCombinersManager.KeyStoreItem Combiners { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static CombinersManagerItem New<TCombiner>()
                        where TCombiner : class, ICrossValueCombiner
        {
            var combiner = Activator.CreateInstance<TCombiner>();

            return new CombinersManagerItem()
            {
                NameCombiners = new CrossNameCombinerManager.KeyStoreItem { Key = FindName(typeof(TCombiner)).GetHashCode(), Value = combiner },
                Combiners = new CrossCombinersManager.KeyStoreItem { Key = typeof(TCombiner).GetHashCode(), Value = combiner }
            };
        }

        public static void RegisterBindersModel(CombinersManagerItem[] items) 
        {
            var nameCombertesManager = new CrossNameCombinerManager(items.Select(x => x.NameCombiners).ToArray());
            var combertersManager = new CrossCombinersManager(items.Select(x => x.Combiners).ToArray());

            Singleton<CrossNameCombinerManager>.Add(nameCombertesManager);
            Singleton<CrossCombinersManager>.Add(combertersManager);
        }

        private static string FindName(Type type)
        {
            var name = type.Name;
            name = RemoveHead(name, "Mvx");
            name = RemoveTail(name, "ValueConverter");
            name = RemoveTail(name, "Converter");
            return name;
        }

        private static string RemoveHead(string name, string word)
        {
            if (name.StartsWith(word))
                name = name[word.Length..];
            return name;
        }

        private static string RemoveTail(string name, string word)
        {
            if (name.EndsWith(word))
                name = name[..^word.Length];
            return name;
        }
    }
}
