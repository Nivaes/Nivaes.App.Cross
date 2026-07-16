using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class ConvertersContainerManagerHelper
    {
        public sealed class ConverterManagerItem
        {
            internal NameConvertersKeyContainerManager.KeyStoreItem NameConverters { [DebuggerHidden] get; [DebuggerHidden] set; }
            internal ConvertersKeyContainerManager.KeyStoreItem Converters { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static ConverterManagerItem New<TConverter>(IServiceProvider services, string name)
                        where TConverter : ICrossValueConverter
        {
            try
            {
                var converter = ActivatorUtilities.CreateInstance<TConverter>(services);

                return new ConverterManagerItem()
                {
                    NameConverters = new NameConvertersKeyContainerManager.KeyStoreItem { Key = name.GetHashCode(), Value = converter },
                    Converters = new ConvertersKeyContainerManager.KeyStoreItem { Key = typeof(TConverter).TypeHandle.Value, Value = converter }
                };
            }
            catch (InvalidOperationException ex)
            {
                throw new CrossException(ex, $"Could not create an instance of type {typeof(TConverter)}");
            }
        }

        public static ConverterManagerItem New<TConverter>(IServiceProvider services)
                        where TConverter : ICrossValueConverter
        {
            return New<TConverter>(services, FindName(typeof(TConverter)));
        }

        public static void RegisterComverters(ConverterManagerItem[] items)
        {
            Singleton<NameConvertersKeyContainerManager>.Instance.Merge(items.Select(x => x.NameConverters).ToArray());
            Singleton<ConvertersKeyContainerManager>.Instance.Merge(items.Select(x => x.NameConverters).ToArray());
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
