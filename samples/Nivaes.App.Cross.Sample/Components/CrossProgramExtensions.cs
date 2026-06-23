using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Win32;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample
{
    public static class CrossProgramExtensions
    {
        public static CrossAppBuilder UseSharedCrossApp(this CrossAppBuilder builder)
        {
            builder.UseCrossApp<SampleApp>();

            builder.SetupConverters();

            return builder;
        }

        static CrossAppBuilder SetupConverters(this CrossAppBuilder builder)
        {
            CrossConvertersManagerHelper.RegisterComverters(new[]
            {
                CrossConvertersManagerHelper.New<StringToLowerValueConverter>(),
                CrossConvertersManagerHelper.New<StringToUpperValueConverter>(),
                CrossConvertersManagerHelper.New<TextToColorValueConverter>(),
            });

            return builder;
        }

        static CrossAppBuilder SetupCombertes(this CrossAppBuilder builder)
        {
            CrossCombinersManagerHelper.RegisterCombiners(new[]
            {
                CrossCombinersManagerHelper.New("Add", new CrossAddValueCombiner()),
                CrossCombinersManagerHelper.New("Add", new CrossAddValueCombiner()),
                CrossCombinersManagerHelper.New("Divide", new CrossDivideValueCombiner()),
                CrossCombinersManagerHelper.New("Format", new CrossFormatValueCombiner()),
                CrossCombinersManagerHelper.New("If", new CrossIfValueCombiner()),
                CrossCombinersManagerHelper.New("Modulus", new CrossModulusValueCombiner()),
                CrossCombinersManagerHelper.New("Multiply", new CrossMultiplyValueCombiner()),
                CrossCombinersManagerHelper.New("Single", new CrossSingleValueCombiner()),
                CrossCombinersManagerHelper.New("Subtract", new CrossSubtractValueCombiner()),
                CrossCombinersManagerHelper.New("EqualTo", new CrossEqualToValueCombiner()),
                CrossCombinersManagerHelper.New("NotEqualTo", new MvxNotEqualToValueCombiner()),
                CrossCombinersManagerHelper.New("GreaterThanOrEqualTo", new CrossGreaterThanOrEqualToValueCombiner()),
                CrossCombinersManagerHelper.New("GreaterThan", new CrossGreaterThanValueCombiner()),
                CrossCombinersManagerHelper.New("LessThanOrEqualTo", new CrossLessThanOrEqualToValueCombiner()),
                CrossCombinersManagerHelper.New("LessThan", new CrossLessThanValueCombiner()),
                CrossCombinersManagerHelper.New("Not", new MvxNotValueCombiner()),
                CrossCombinersManagerHelper.New("And", new MvxAndValueCombiner()),
                CrossCombinersManagerHelper.New("Or", new MvxOrValueCombiner()),
                CrossCombinersManagerHelper.New("XOr", new MvxXorValueCombiner()),
                CrossCombinersManagerHelper.New("Inverted", new MvxInvertedValueCombiner()),
            });

            return builder;
        }
    }
}
