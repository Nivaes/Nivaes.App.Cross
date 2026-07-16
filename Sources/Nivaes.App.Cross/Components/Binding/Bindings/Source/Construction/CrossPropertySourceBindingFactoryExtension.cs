namespace Nivaes.App.Cross
{
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    /// <summary>
    /// Uses a global cache of calls in Reflection namespace
    /// </summary>
    [Obsolete]
    public class CrossPropertySourceBindingFactoryExtension
        : ICrossSourceBindingFactoryExtension
    {
        private readonly ConcurrentDictionary<int, PropertyInfo> _propertyInfoCache = new();

        [RequiresUnreferencedCode("This method creates source bindings which use reflection and may not be preserved by trimming")]
        public bool TryCreateBinding(
            object? source,
            ICrossPropertyToken propertyToken,
            List<ICrossPropertyToken> remainingTokens,
            out ICrossSourceBinding? result)
        {
            if (source == null)
            {
                result = null;
                return false;
            }

            result = remainingTokens.Count == 0
                ? CreateLeafBinding(source, propertyToken)
                : CreateChainedBinding(source, propertyToken, remainingTokens);

            return result != null;
        }

        [RequiresUnreferencedCode("This method creates chained source bindings which use reflection and may not be preserved by trimming")]
        protected virtual CrossChainedSourceBinding? CreateChainedBinding(
            object source,
            ICrossPropertyToken propertyToken,
            List<ICrossPropertyToken> remainingTokens)
        {
            switch (propertyToken)
            {
                case CrossIndexerPropertyToken indexPropertyToken:
                    {
                        var itemPropertyInfo = FindPropertyInfo(source);
                        if (itemPropertyInfo == null)
                            return null;

                        return new CrossIndexerChainedSourceBinding(source, itemPropertyInfo, indexPropertyToken,
                            remainingTokens);
                    }
                case CrossPropertyNamePropertyToken propertyNameToken:
                    {
                        var propertyInfo = FindPropertyInfo(source, propertyNameToken.PropertyName);

                        if (propertyInfo == null)
                            return null;

                        return new CrossSimpleChainedSourceBinding(source, propertyInfo,
                            remainingTokens);
                    }
                default:
                    throw new AppException("Unexpected property chaining - seen token type {0}",
                        propertyToken.GetType().FullName);
            }
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected virtual ICrossSourceBinding? CreateLeafBinding(object source, ICrossPropertyToken propertyToken)
        {
            if (propertyToken is CrossIndexerPropertyToken indexPropertyToken)
            {
                var itemPropertyInfo = FindPropertyInfo(source);
                if (itemPropertyInfo == null)
                    return null;
                return new CrossIndexerLeafPropertyInfoSourceBinding(source, itemPropertyInfo, indexPropertyToken);
            }

            if (propertyToken is CrossPropertyNamePropertyToken propertyNameToken)
            {
                var propertyInfo = FindPropertyInfo(source, propertyNameToken.PropertyName);
                if (propertyInfo == null)
                    return null;
                return new CrossSimpleLeafPropertyInfoSourceBinding(source, propertyInfo);
            }

            if (propertyToken is CrossEmptyPropertyToken)
            {
                return new CrossDirectToSourceBinding(source);
            }

            throw new AppException("Unexpected property source - seen token type {0}", propertyToken.GetType().FullName);
        }

        protected PropertyInfo? FindPropertyInfo<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(T? source, string propertyName = "Item")
        {
            var sourceType = source?.GetType();
            if (sourceType == null)
                return null;

            var key = (sourceType.FullName + "." + propertyName).GetHashCode();

            if (_propertyInfoCache.TryGetValue(key, out PropertyInfo? pi))
                return pi;

            // Get lowest property
            while (sourceType != null)
            {
                // Use BindingFlags.DeclaredOnly to avoid AmbiguousMatchException
                pi = sourceType.GetProperty(propertyName,
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                if (pi != null)
                {
                    break;
                }
                sourceType = sourceType.BaseType;
            }

            if (pi != null)
                _propertyInfoCache.TryAdd(key, pi);

            return pi;
        }
    }
}