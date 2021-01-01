// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MvvmCross.Exceptions;
    using MvvmCross.IoC;
    using Nivaes.App.Cross;

    public class MvxViewModelViewLookupBuilder
        : IMvxTypeToTypeLookupBuilder
    {
        public virtual IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies)
        {
            var associatedTypeFinder = Mvx.IoCProvider.Resolve<IMvxViewModelTypeFinder>();

            var views = from assembly in sourceAssemblies
                        from candidateViewType in assembly.ExceptionSafeGetTypes()
                        let viewModelType = associatedTypeFinder.FindTypeOrNull(candidateViewType)
                        where viewModelType != null
                        select new KeyValuePair<Type, Type>(viewModelType, candidateViewType);

            var aa = views.Select(a => a.Value.FullName).ToArray();

            try
            {
                return views.ToDictionary(x => x.Key, x => x.Value);
            }
            catch (ArgumentException ex)
            {
                throw ReportBuildProblem(views, ex);
            }
        }

        private Exception ReportBuildProblem(IEnumerable<KeyValuePair<Type, Type>> views, ArgumentException ex)
        {
            var overSizedCounts = views.GroupBy(x => x.Key)
                                       .Select(x => new { x.Key.Name, Count = x.Count(), ViewNames = x.Select(v => v.Value.Name).ToList() })
                                       .Where(x => x.Count > 1)
                                       .Select(x => $"{x.Count}*{x.Name} ({string.Join(",", x.ViewNames)})")
                                       .ToArray();

            if (overSizedCounts.Length == 0)
            {
                // no idea what the error is - so throw the original
                return new MvxException("Unknown problem in ViewModelViewLookup construction", ex);
            }
            else
            {
                var overSizedText = string.Join(";", overSizedCounts);

                return new MvxException($"Problem seen creating View-ViewModel lookup table - you have more than one View registered for the ViewModels: '{overSizedText}'", ex);
            }
        }
    }
}
