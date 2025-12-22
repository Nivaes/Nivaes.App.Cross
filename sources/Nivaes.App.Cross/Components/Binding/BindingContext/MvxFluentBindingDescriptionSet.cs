namespace MvvmCross.Binding.BindingContext
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Base;
    using MvvmCross.Binding.Bindings;
    using Nivaes.App.Cross;

    public class MvxFluentBindingDescriptionSet<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TOwningTarget, TSource>
            : CrossApplicable, IDisposable
                where TOwningTarget : class, IMvxBindingContextOwner
    {
        private readonly List<ICrossApplicable> _applicables = [];
        private readonly TOwningTarget _bindingContextOwner;
        private readonly string _clearBindingKey;

        public MvxFluentBindingDescriptionSet(TOwningTarget bindingContextOwner)
        {
            _bindingContextOwner = bindingContextOwner;
        }
        public MvxFluentBindingDescriptionSet(TOwningTarget bindingContextOwner, string clearBindingKey) : this(bindingContextOwner)
        {
            _clearBindingKey = clearBindingKey;
        }
        public MvxFluentBindingDescription<TOwningTarget, TSource> Bind()
        {
            var toReturn = new MvxFluentBindingDescription<TOwningTarget, TSource>(
                _bindingContextOwner, _bindingContextOwner);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(TChildTarget childTarget)
                where TChildTarget : class
        {
            var toReturn = new MvxFluentBindingDescription<TChildTarget, TSource>(_bindingContextOwner, childTarget);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(
                TChildTarget childTarget,
                string bindingDescription)
                    where TChildTarget : class
        {
            var toReturn = Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(
                TChildTarget childTarget, CrossBindingDescription bindingDescription)
                    where TChildTarget : class
        {
            var toReturn = Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public override void Apply()
        {
            foreach (var applicable in _applicables)
                applicable.Apply();
            base.Apply();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public void ApplyWithClearBindingKey(object clearBindingKey)
        {
            foreach (var applicable in _applicables)
            {
                if (applicable is IMvxBaseFluentBindingDescription fluentBindingDescription)
                {
                    fluentBindingDescription.ClearBindingKey = clearBindingKey;
                }
                else
                {
                    MvxBindingLog.Instance?.LogWarning(
                        "Fluent binding description must implement {InterfaceName} in order to add {Description}",
                        nameof(IMvxBaseFluentBindingDescription),
                        nameof(IMvxBaseFluentBindingDescription.ClearBindingKey));
                }

                applicable.Apply();
            }

            base.Apply();
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Bindings inherently use reflection. This is by design and callers are warned through usage of binding methods.")]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (string.IsNullOrEmpty(_clearBindingKey))
                    Apply();
                else
                    ApplyWithClearBindingKey(_clearBindingKey);
            }
        }
    }
}
