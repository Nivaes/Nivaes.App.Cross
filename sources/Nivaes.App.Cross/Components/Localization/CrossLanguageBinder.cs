namespace Nivaes.App.Cross
{
    using System;

    public class CrossLanguageBinder(string? namespaceName = null, string? typeName = null)
        : ICrossLanguageBinder
    {
        private readonly object _lockObject = new();
        private ICrossTextProvider? _cachedTextProvider;

        public CrossLanguageBinder(Type owningObject)
            : this(owningObject.Namespace, owningObject.Name)
        {
        }

        protected virtual ICrossTextProvider? GetTextProvider()
        {
            throw new NotImplementedException();
            //lock (_lockObject)
            //{
            //    if (_cachedTextProvider != null)
            //        return _cachedTextProvider;

            //    if (Mvx.IoCProvider?.TryResolve(out ICrossTextProvider? cachedTextProvider) != true)
            //    {
            //        throw new MvxException(
            //            "Missing text provider - please initialize IoC with a suitable IMvxTextProvider");
            //    }

            //    return _cachedTextProvider = cachedTextProvider;
            //}
        }

        public virtual string? GetText(string entryKey)
        {
            return GetText(namespaceName, typeName, entryKey);
        }

        public virtual string? GetText(string entryKey, params object[] args)
        {
            var format = GetText(entryKey);
            return format == null ? null : string.Format(format, args);
        }

        protected virtual string? GetText(string? namespaceKey, string? typeKey, string entryKey)
        {
            return GetTextProvider()?.GetText(namespaceKey, typeKey, entryKey);
        }
    }
}