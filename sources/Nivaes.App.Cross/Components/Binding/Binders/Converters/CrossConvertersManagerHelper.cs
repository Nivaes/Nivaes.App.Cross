using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public static class CrossConvertersManagerHelper
    {
        public sealed class ConverterManagerItem
        {
            internal CrossNameConvertersManager.KeyStoreItem NameConverters { [DebuggerHidden]get; [DebuggerHidden]set; }
            internal CrossConvertersManager.KeyStoreItem Converters { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static ConverterManagerItem New<TConverter>()
                        where TConverter : class, ICrossValueConverter
        {
            var converter = Activator.CreateInstance<TConverter>();

            return new ConverterManagerItem()
            {                
                NameConverters = new CrossNameConvertersManager.KeyStoreItem { Key = FindName(typeof(TConverter)).GetHashCode(), Value = converter },
                Converters = new CrossConvertersManager.KeyStoreItem { Key = typeof(TConverter).GetHashCode(), Value = converter }
            };
        }

        public static void RegisterComverters(ConverterManagerItem[] items) 
        {
            var nameConvertesManager = new CrossNameConvertersManager(items.Select(x => x.NameConverters).ToArray());
            var convertesManager = new CrossConvertersManager(items.Select(x => x.NameConverters).ToArray());

            Singleton<CrossNameConvertersManager>.Add(nameConvertesManager);
            Singleton<CrossConvertersManager>.Add(convertesManager);
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
