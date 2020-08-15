// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Converters;
using MvvmCross.IoC;

namespace MvvmCross.Binding.Combiners
{
    [MvxUnconventional]
    public class MvxValueConverterValueCombiner : MvxValueCombiner
    {
        private readonly IMvxValueConverter mValueConverter;

        public MvxValueConverterValueCombiner(IMvxValueConverter valueConverter)
        {
            mValueConverter = valueConverter;
        }

        public override void SetValue(IEnumerable<IMvxSourceStep> steps, object value)
        {
            var sourceStep = steps.First();
            var parameter = GetParameterValue(steps);

            if (mValueConverter == null)
            {
                // null value converter always fails
                return;
            }
            var converted = mValueConverter.ConvertBack(value, sourceStep.SourceType, parameter,
                                                        CultureInfo.CurrentUICulture);
            sourceStep.SetValue(converted);
        }

        private Type mTargetType = typeof(object);

        public override IEnumerable<Type> SubStepTargetTypes(IEnumerable<IMvxSourceStep> subSteps, Type overallTargetType)
        {
            mTargetType = overallTargetType;
            return base.SubStepTargetTypes(subSteps, overallTargetType);
        }

        private static object? GetParameterValue(IEnumerable<IMvxSourceStep> steps)
        {
            var parameterStep = steps.Skip(1).FirstOrDefault();
            object? parameter = null;
            if (parameterStep != null)
            {
                parameter = parameterStep.GetValue();
            }
            return parameter;
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var sourceStep = steps.First();
            var parameter = GetParameterValue(steps);

            object sourceValue = sourceStep.GetValue();
            if (sourceValue == MvxBindingConstant.DoNothing)
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (sourceValue == MvxBindingConstant.UnsetValue)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            if (mValueConverter == null)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            value = mValueConverter.Convert(sourceValue, mTargetType, parameter, CultureInfo.CurrentUICulture);
            return true;
        }
    }
}
