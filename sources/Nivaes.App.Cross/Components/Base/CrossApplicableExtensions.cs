namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static class CrossApplicableExtensions
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void Apply(this IEnumerable<ICrossApplicable> toApply)
        {
            ArgumentNullException.ThrowIfNull(toApply, nameof(toApply));

            foreach (var applicable in toApply)
                applicable.Apply();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void ApplyTo(this IEnumerable<ICrossApplicableTo> toApply, object what)
        {
            if (toApply == null)
                throw new ArgumentNullException(nameof(toApply));

            foreach (var applicable in toApply)
                applicable.ApplyTo(what);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void ApplyTo<T>(this IEnumerable<IMvxApplicableTo<T>> toApply, T what)
            where T : notnull
        {
            ArgumentNullException.ThrowIfNull(toApply, nameof(toApply));

            foreach (var applicable in toApply)
                applicable.ApplyTo(what);
        }
    }
#nullable restore
}
