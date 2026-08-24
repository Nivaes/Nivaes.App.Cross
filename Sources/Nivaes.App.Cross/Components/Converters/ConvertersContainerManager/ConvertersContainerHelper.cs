using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class ConvertersContainerHelper
    {
        public sealed class ConverterManagerItem
        {
            required public string Name;

            required public Type Type;

            required public ICrossValueConverter Converter;
        }

        public static ConverterManagerItem New<TConverter>(IServiceProvider services, string name)
                        where TConverter : ICrossValueConverter
        {
            try
            {
                var converter = ActivatorUtilities.CreateInstance<TConverter>(services);

                return new ConverterManagerItem
                {
                    Name = name,
                    Type = typeof(TConverter),
                    Converter = converter
                };
            }
            catch (InvalidOperationException ex)
            {
                throw new AppException(ex, $"Could not create an instance of type {typeof(TConverter)}");
            }
        }

        public static ConverterManagerItem New<TConverter>(IServiceProvider services)
                        where TConverter : ICrossValueConverter
        {
            return New<TConverter>(services, FindName(typeof(TConverter)));
        }

        public static void RegisterComverters(ConverterManagerItem[] items)
        {
            var containers = Singleton<ConvertersContainers>.Instance;

            foreach (var item in items) 
            {
                containers.NameConverters.Add(item.Name, item.Converter);
                containers.Converters.Add(item.Type, item.Converter);
            }
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
