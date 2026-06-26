using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class CrossConvertersManagerHelper
    {
        public sealed class ConverterManagerItem
        {
            internal CrossNameConvertersManager.KeyStoreItem NameConverters { [DebuggerHidden] get; [DebuggerHidden] set; }
            internal CrossConvertersManager.KeyStoreItem Converters { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static ConverterManagerItem New<TConverter>(IServiceProvider services, string name)
                        where TConverter : class, ICrossValueConverter
        {
            var converter = ActivatorUtilities.CreateInstance<TConverter>(services);

            return new ConverterManagerItem()
            {
                NameConverters = new CrossNameConvertersManager.KeyStoreItem { Key = name.GetHashCode(), Value = converter },
                Converters = new CrossConvertersManager.KeyStoreItem { Key = typeof(TConverter).GetHashCode(), Value = converter }
            };
        }

        public static ConverterManagerItem New<TConverter>(IServiceProvider services)
                        where TConverter : class, ICrossValueConverter
        {
            return New<TConverter>(services, FindName(typeof(TConverter)));
        }

        public static void RegisterComverters(ConverterManagerItem[] items)
        {
            Singleton<CrossNameConvertersManager>.Instance.Merge(items.Select(x => x.NameConverters).ToArray());
            Singleton<CrossConvertersManager>.Instance.Merge(items.Select(x => x.NameConverters).ToArray());
        }

        private static string FindName(Type type)
        {
            var name = type.Name;
            name = RemoveHead(name, "Cross");
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
