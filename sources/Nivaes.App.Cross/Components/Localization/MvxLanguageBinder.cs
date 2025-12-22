namespace MvvmCross.Localization
{
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;

    public class MvxLanguageBinder(string? namespaceName = null, string? typeName = null)
        : IMvxLanguageBinder
    {
        private readonly object _lockObject = new();
        private IMvxTextProvider? _cachedTextProvider;

        public MvxLanguageBinder(Type owningObject)
            : this(owningObject.Namespace, owningObject.Name)
        {
        }

        protected virtual IMvxTextProvider? GetTextProvider()
        {
            lock (_lockObject)
            {
                if (_cachedTextProvider != null)
                    return _cachedTextProvider;

                if (Mvx.IoCProvider?.TryResolve(out IMvxTextProvider? cachedTextProvider) != true)
                {
                    throw new CrossException(
                        "Missing text provider - please initialize IoC with a suitable IMvxTextProvider");
                }

                return _cachedTextProvider = cachedTextProvider;
            }
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