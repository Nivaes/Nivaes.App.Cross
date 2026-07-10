using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross
{
    [CrossValueCombiner(Name = "Single")]
    public class CrossSingleValueCombiner
        : CrossValueCombiner
    {
        public CrossSingleValueCombiner()
            :base(CrossLoggerHost.GetLogger<CrossSingleValueCombiner>())
        { }

        [ActivatorUtilitiesConstructor]
        public CrossSingleValueCombiner(ILogger<CrossSingleValueCombiner> logger)
            : base(logger)
        {
        }
        public override Type SourceType(IEnumerable<ICrossSourceStep> steps)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
                return typeof(object);

            return firstStep.SourceType;
        }

        public override void SetValue(IEnumerable<ICrossSourceStep> steps, object? value)
        {
            var firstStep = steps.FirstOrDefault();

            firstStep?.SetValue(value);
        }

        public override bool TryGetValue(IEnumerable<ICrossSourceStep> steps, out object? value)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
            {
                value = CrossBindingConstant.UnsetValue;
                return true;
            }

            value = firstStep.GetValue();
            return true;
        }
    }
}
