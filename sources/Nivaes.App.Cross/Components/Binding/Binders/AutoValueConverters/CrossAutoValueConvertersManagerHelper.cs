using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public static class CrossAutoValueConvertersManagerHelper
    {
        public sealed class AutoValueConvertersManagerItem
        {
            internal CrossAutoValueConvertersManager.KeyStoreItem AutoValueConvertes { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static AutoValueConvertersManagerItem New(Type viewModelType, Type viewType, ICrossValueConverter converters)
        {
            var key = (viewModelType.GetType(),viewType.GetType()).GetHashCode();
            return new AutoValueConvertersManagerItem()
            {
                AutoValueConvertes = new CrossAutoValueConvertersManager.KeyStoreItem { Key = key, Value = converters }
            };
        }

        public static void RegisterCombiners(AutoValueConvertersManagerItem[] items) 
        {
            Singleton<CrossAutoValueConvertersManager>.Instance.Merge(items.Select(x => x.AutoValueConvertes).ToArray());
        }
    }
}
