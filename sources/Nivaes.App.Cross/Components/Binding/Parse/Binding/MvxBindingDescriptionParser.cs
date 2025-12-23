namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.Parse.Binding.Lang;
    using MvvmCross.Binding.Parse.Binding.Tibet;

    public class MvxBindingDescriptionParser
        : ICrossBindingDescriptionParser
    {
        private IMvxBindingParser _bindingParser;
        private ICrossValueConverterLookup _valueConverterLookup;

        protected IMvxBindingParser BindingParser
        {
            get
            {
                _bindingParser ??= Mvx.IoCProvider.Resolve<IMvxBindingParser>();
                return _bindingParser;
            }
        }

        private IMvxLanguageBindingParser _languageBindingParser;

        protected IMvxLanguageBindingParser LanguageBindingParser
        {
            get
            {
                _languageBindingParser ??= Mvx.IoCProvider.Resolve<IMvxLanguageBindingParser>();
                return _languageBindingParser;
            }
        }

        protected ICrossValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup ??= Mvx.IoCProvider.Resolve<ICrossValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        protected IMvxValueConverter FindConverter(string converterName)
        {
            if (converterName == null)
                return null;

            var toReturn = ValueConverterLookup.Find(converterName);
            if (toReturn == null)
                MvxBindingLog.Instance?.LogTrace("Could not find named converter for {ConverterName}", converterName);

            return toReturn;
        }

        protected IMvxValueCombiner FindCombiner(string combiner)
        {
            return MvxBindingSingletonCache.Instance?.ValueCombinerLookup.Find(combiner);
        }

        public IEnumerable<CrossBindingDescription> Parse(string text)
        {
            var parser = BindingParser;
            return Parse(text, parser);
        }

        public IEnumerable<CrossBindingDescription> Parse(string text, IMvxBindingParser parser)
        {
            MvxSerializableBindingSpecification specification;
            if (!parser.TryParseBindingSpecification(text, out specification))
            {
                MvxBindingLog.Instance?.LogError("Failed to parse binding description starting with {BindingText}",
                    GetErrorTextParameter(text));
                return Array.Empty<CrossBindingDescription>();
            }

            if (specification == null)
                return Array.Empty<CrossBindingDescription>();

            return from item in specification
                   select SerializableBindingToBinding(item.Key, item.Value);
        }

        public IEnumerable<CrossBindingDescription> LanguageParse(string text)
        {
            var parser = LanguageBindingParser;
            return Parse(text, parser);
        }

        public CrossBindingDescription ParseSingle(string text)
        {
            MvxSerializableBindingDescription description;
            var parser = BindingParser;
            if (!parser.TryParseBindingDescription(text, out description))
            {
                MvxBindingLog.Instance?.LogError("Failed to parse binding description starting with {BindingText}",
                    GetErrorTextParameter(text));
                return null;
            }

            if (description == null)
                return null;

            return SerializableBindingToBinding(null, description);
        }

        private static string GetErrorTextParameter(string text)
        {
            if (text == null)
                return string.Empty;

            if (text.Length > 20)
                return text[..20];

            return text;
        }

        public CrossBindingDescription SerializableBindingToBinding(
            string targetName, MvxSerializableBindingDescription description)
        {
            return new CrossBindingDescription
            {
                TargetName = targetName,
                Source = SourceStepDescriptionFrom(description),
                Mode = description.Mode,
            };
        }

        private MvxSourceStepDescription SourceStepDescriptionFrom(MvxSerializableBindingDescription description)
        {
            if (description.Path != null)
            {
                return new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = description.Path,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    FallbackValue = description.FallbackValue
                };
            }

            if (description.Literal != null)
            {
                var literal = description.Literal;
                if (literal == MvxTibetBindingParser.LiteralNull)
                    literal = null;

                return new MvxLiteralSourceStepDescription()
                {
                    Literal = literal,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    FallbackValue = description.FallbackValue
                };
            }

            if (description.Function != null)
            {
                // first look for a combiner with the name
                var combiner = FindCombiner(description.Function);
                if (combiner != null)
                {
                    return new MvxCombinerSourceStepDescription()
                    {
                        Combiner = combiner,
                        InnerSteps = description.Sources == null
                            ? new List<MvxSourceStepDescription>() :
                            description.Sources.Select(s => SourceStepDescriptionFrom(s)).ToList(),
                        Converter = FindConverter(description.Converter),
                        ConverterParameter = description.ConverterParameter,
                        FallbackValue = description.FallbackValue
                    };
                }
                else
                {
                    // no combiner, then drop back to looking for a converter
                    var converter = FindConverter(description.Function);
                    if (converter == null)
                    {
                        MvxBindingLog.Instance?.LogError("Failed to find combiner or converter for {FunctionName}",
                            description.Function);
                    }

                    if (description.Sources == null || description.Sources.Count == 0)
                    {
                        MvxBindingLog.Instance?.LogError("Value Converter {FunctionName} supplied with no source",
                            description.Function);
                        return new MvxLiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else if (description.Sources.Count > 2)
                    {
                        MvxBindingLog.Instance?.LogError(
                            "Value Converter {FunctionName} supplied with too many parameters - {ParameterCount}",
                            description.Function, description.Sources.Count);
                        return new MvxLiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else
                    {
                        return new MvxCombinerSourceStepDescription()
                        {
                            Combiner = new MvxValueConverterValueCombiner(converter),
                            InnerSteps = description.Sources.Select(source => SourceStepDescriptionFrom(source)).ToList(),
                            Converter = FindConverter(description.Converter),
                            ConverterParameter = description.ConverterParameter,
                            FallbackValue = description.FallbackValue
                        };
                    }
                }
            }

            // this probably suggests that the path is the entire source object
            return new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = null,
                Converter = FindConverter(description.Converter),
                ConverterParameter = description.ConverterParameter,
                FallbackValue = description.FallbackValue
            };
        }
    }
}
