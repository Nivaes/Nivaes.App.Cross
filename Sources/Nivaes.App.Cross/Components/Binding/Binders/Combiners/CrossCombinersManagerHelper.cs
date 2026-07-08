using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public static class CrossCombinersManagerHelper
    {
        public sealed class CombinersManagerItem
        {
            internal CrossNameCombinersManager.KeyStoreItem NameCombiners { [DebuggerHidden] get; [DebuggerHidden] set; }
            internal CrossCombinersManager.KeyStoreItem Combiners { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static CombinersManagerItem New(ICrossValueCombiner combiner, string name)
        {
            return new CombinersManagerItem()
            {
                NameCombiners = new CrossNameCombinersManager.KeyStoreItem { Key = name.GetHashCode(), Value = combiner },
                Combiners = new CrossCombinersManager.KeyStoreItem { Key = combiner.GetType().GetHashCode(), Value = combiner }
            };
        }

        public static void RegisterCombiners(CombinersManagerItem[] items)
        {
            Singleton<CrossNameCombinersManager>.Instance.Merge(items.Select(x => x.NameCombiners).ToArray());
            Singleton<CrossCombinersManager>.Instance.Merge(items.Select(x => x.Combiners).ToArray());
        }
    }
}
