using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class CombinersContainerHelper
    {
        public sealed class CombinersManagerItem
        {
            required public string NameCombiner;
            required public Type TypeCombiner;
            required public ICrossValueCombiner Combiner;
        }

        public static CombinersManagerItem New<TCombiner>(IServiceProvider services, string name)
            where TCombiner : ICrossValueCombiner
        {
            try
            {
                return new CombinersManagerItem
                {
                    NameCombiner = name,
                    TypeCombiner = typeof(TCombiner),
                    Combiner = ActivatorUtilities.CreateInstance<TCombiner>(services)
                };
            }
            catch (InvalidOperationException ex)
            {
                throw new AppException(ex, $"Could not create an instance of type {typeof(TCombiner)}");
            }
        }

        public static CombinersManagerItem New<TCombiner>(IServiceProvider services)
                    where TCombiner : ICrossValueCombiner
        {
            return New<TCombiner>(services, FindName(typeof(TCombiner)));
        }

        public static void RegisterCombiners(CombinersManagerItem[] items)
        {
            var combinersContainer = Singleton<CombinersContainers>.Instance;

            foreach(var item in items)
            {
                combinersContainer.NameCombiners.Add(item.NameCombiner, item.Combiner);
                combinersContainer.Combiners.Add(item.TypeCombiner, item.Combiner);
            }
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
