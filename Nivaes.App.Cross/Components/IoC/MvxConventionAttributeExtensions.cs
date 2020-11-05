﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.IoC
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class MvxConventionAttributeExtensions
    {
        /// <summary>
        /// A type is conventional if and only it is:
        /// - not marked with an unconventional attribute
        /// - all marked conditional conventions return true
        /// </summary>
        /// <param name="candidateType"></param>
        /// <returns></returns>
        public static bool IsConventional(this Type candidateType)
        {
            if (candidateType == null) throw new ArgumentNullException(nameof(candidateType));

            var unconventionalAttributes = candidateType.GetCustomAttributes(typeof(MvxUnconventionalAttribute), true);
            if (unconventionalAttributes.Length > 0)
                return false;

            return candidateType.SatisfiesConditionalConventions();
        }

        public static bool SatisfiesConditionalConventions(this Type candidateType)
        {
            if (candidateType == null) throw new ArgumentNullException(nameof(candidateType));

            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof(MvxConditionalConventionalAttribute), true);

            foreach (MvxConditionalConventionalAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConditionSatisfied;
                if (!result)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// A propertyInfo is conventional if and only it is:
        /// - not marked with an unconventional attribute
        /// - all marked conditional conventions return true
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsConventional(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            var unconventionalAttributes = propertyInfo.GetCustomAttributes(typeof(MvxUnconventionalAttribute), true);
            if (unconventionalAttributes.Any())
                return false;

            return propertyInfo.SatisfiesConditionalConventions();
        }

        public static bool SatisfiesConditionalConventions(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            var conditionalAttributes =
                propertyInfo.GetCustomAttributes(typeof(MvxConditionalConventionalAttribute), true);

            foreach (MvxConditionalConventionalAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConditionSatisfied;
                if (!result)
                    return false;
            }
            return true;
        }
    }
}
