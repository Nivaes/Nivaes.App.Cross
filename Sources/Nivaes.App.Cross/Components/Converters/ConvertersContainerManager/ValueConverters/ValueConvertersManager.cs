using System.Diagnostics;

namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public static class ValueConvertersManager
    {
        public sealed class AutoValueConvertersManagerItem
        {
            internal ValueKeyConvertersManager.KeyStoreItem AutoValueConvertes { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static AutoValueConvertersManagerItem New(Type viewModelType, Type viewType, ICrossValueConverter converters)
        {
            var key = (viewModelType.GetType(), viewType.GetType()).GetHashCode();
            return new AutoValueConvertersManagerItem()
            {
                AutoValueConvertes = new ValueKeyConvertersManager.KeyStoreItem { Key = key, Value = converters }
            };
        }

        public static void RegisterCombiners(AutoValueConvertersManagerItem[] items)
        {
            Singleton<ValueKeyConvertersManager>.Instance.Merge(items.Select(x => x.AutoValueConvertes).ToArray());
        }
    }
}
