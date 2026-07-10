using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class CrossCombinersManagerHelper
    {
        public sealed class CombinersManagerItem
        {
            internal CrossNameCombinersManager.KeyStoreItem NameCombiners { [DebuggerHidden] get; [DebuggerHidden] set; }
            internal CrossCombinersManager.KeyStoreItem Combiners { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static CombinersManagerItem New<TCombiner>(IServiceProvider services, string name)
            where TCombiner : ICrossValueCombiner
        {
            var combiner = ActivatorUtilities.CreateInstance<TCombiner>(services);

            return new CombinersManagerItem()
            {
                NameCombiners = new CrossNameCombinersManager.KeyStoreItem { Key = name.GetHashCode(), Value = combiner },
                Combiners = new CrossCombinersManager.KeyStoreItem { Key = combiner.GetType().GetHashCode(), Value = combiner }
            };
        }

        public static CombinersManagerItem New<TCombiner>(IServiceProvider services)
                    where TCombiner : ICrossValueCombiner
        {
            return New<TCombiner>(services, FindName(typeof(TCombiner)));
        }

        public static void RegisterCombiners(CombinersManagerItem[] items)
        {
            Singleton<CrossNameCombinersManager>.Instance.Merge(items.Select(x => x.NameCombiners).ToArray());
            Singleton<CrossCombinersManager>.Instance.Merge(items.Select(x => x.Combiners).ToArray());
        }

        private static string FindName(Type type)
        {
            var name = type.Name;
            name = RemoveHead(name, "Cross");
            name = RemoveTail(name, "ValueCombiner");
            name = RemoveTail(name, "Combiner");
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
