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

        public static CombinersManagerItem New(string name, ICrossValueCombiner combiner)
        {
            return new CombinersManagerItem()
            {
                NameCombiners = new CrossNameCombinerManager.KeyStoreItem { Key =name.GetHashCode(), Value = combiner },
                Combiners = new CrossCombinersManager.KeyStoreItem { Key = combiner.GetType().GetHashCode(), Value = combiner }
            };
        }

        public static void RegisterCombiners(CombinersManagerItem[] items) 
        {
            var nameCombertesManager = new CrossNameCombinerManager(items.Select(x => x.NameCombiners).ToArray());
            var combertersManager = new CrossCombinersManager(items.Select(x => x.Combiners).ToArray());

            Singleton<CrossNameCombinerManager>.Add(nameCombertesManager);
            Singleton<CrossCombinersManager>.Add(combertersManager);
        }
    }
}
