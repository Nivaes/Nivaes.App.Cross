namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using MvvmCross.IoC;

    //[MvxUnconventional]
    public class CrossValueConverterValueCombiner 
        : CrossValueCombiner
    {
        private readonly ICrossValueConverter _valueConverter;

        public CrossValueConverterValueCombiner(ICrossValueConverter valueConverter)
        {
             _valueConverter = valueConverter;
        }

        public override void SetValue(IEnumerable<ICrossSourceStep> steps, object value)
        {
            var sourceStep = steps.First();
            var parameter = GetParameterValue(steps);

            if (_valueConverter == null)
            {
                // null value converter always fails
                return;
            }

            var converted = _valueConverter.ConvertBack(value, sourceStep.SourceType, parameter,
                CultureInfo.CurrentUICulture);
            sourceStep.SetValue(converted);
        }

        private Type _targetType = typeof(object);

        public override IEnumerable<Type> SubStepTargetTypes(IEnumerable<ICrossSourceStep> subSteps, Type overallTargetType)
        {
            _targetType = overallTargetType;
            return base.SubStepTargetTypes(subSteps, overallTargetType);
        }

        private static object? GetParameterValue(IEnumerable<ICrossSourceStep> steps)
        {
            var parameterStep = steps.Skip(1).FirstOrDefault();
            object? parameter = null;
            if (parameterStep != null)
            {
                parameter = parameterStep.GetValue();
            }
            return parameter;
        }

        public override bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object value)
        {
            var sourceStep = steps.First();
            var parameter = GetParameterValue(steps);

            object sourceValue = sourceStep.GetValue();
            if (sourceValue == CrossBindingConstant.DoNothing)
            {
                value = CrossBindingConstant.DoNothing;
                return true;
            }

            if (sourceValue == CrossBindingConstant.UnsetValue)
            {
                value = CrossBindingConstant.UnsetValue;
                return true;
            }

            if (_valueConverter == null)
            {
                value = CrossBindingConstant.UnsetValue;
                return true;
            }

            value = _valueConverter.Convert(sourceValue, _targetType, parameter, CultureInfo.CurrentUICulture);
            return true;
        }
    }
}
