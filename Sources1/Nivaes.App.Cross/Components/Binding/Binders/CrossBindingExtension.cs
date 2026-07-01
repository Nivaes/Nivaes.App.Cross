using Nivaes.App.Cross.Visibility;

namespace Nivaes.App.Cross
{
    public static class CrossBindingExtension
    {
        public static IServiceProvider SetupConverters(this IServiceProvider services)
        {
            CrossConvertersManagerHelper.RegisterComverters(new[]
            {
                CrossConvertersManagerHelper.New<CrossARGBValueConverter>(services, "ARGB"),
                CrossConvertersManagerHelper.New<CrossNativeColorValueConverter>(services, "NativeColor"),
                CrossConvertersManagerHelper.New<CrossRGBAValueConverter>(services, "RGBA"),
                CrossConvertersManagerHelper.New<CrossRGBValueConverter>(services, "RGB"),
                CrossConvertersManagerHelper.New<CrossRGBIntColorValueConverter>(services, "RGBIntColor"),

                CrossConvertersManagerHelper.New<CrossCommandParameterValueConverter>(services, "CommandParameter"),
                CrossConvertersManagerHelper.New<CrossLanguageConverter>(services, "Language"),

                CrossConvertersManagerHelper.New<CrossVisibilityValueConverter>(services, "Visibility"),
                CrossConvertersManagerHelper.New<CrossInvertedVisibilityValueConverter>(services, "InvertedVisibility"),
        });

            return services;
        }

        public static IServiceProvider SetupCombertes(this IServiceProvider services)
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

            return services;
        }
    }
}
