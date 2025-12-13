namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    public class CrossBindingDescriptionParser
        : ICrossBindingDescriptionParser
    {
        private ICrossBindingParser _bindingParser;
        private ICrossValueConverterLookup _valueConverterLookup;

        protected ICrossBindingParser BindingParser
        {
            get
            {
                throw new NotImplementedException();
                //_bindingParser ??= Cross.IoCProvider.Resolve<ICrossBindingParser>();
                //return _bindingParser;
            }
        }

        private ICrossLanguageBindingParser _languageBindingParser;

        protected ICrossLanguageBindingParser LanguageBindingParser
        {
            get
            {
                throw new NotImplementedException();
                //_languageBindingParser ??= Cross.IoCProvider.Resolve<ICrossLanguageBindingParser>();
                //return _languageBindingParser;
            }
        }

        protected ICrossValueConverterLookup ValueConverterLookup
        {
            get
            {
                throw new NotImplementedException();

                //_valueConverterLookup ??= Cross.IoCProvider.Resolve<ICrossValueConverterLookup>();
                //return _valueConverterLookup;
            }
        }

        protected ICrossValueConverter FindConverter(string converterName)
        {
            if (converterName == null)
                return null;

            var toReturn = ValueConverterLookup.Find(converterName);
            if (toReturn == null)
                CrossBindingLog.Instance?.LogTrace("Could not find named converter for {ConverterName}", converterName);

            return toReturn;
        }

        protected ICrossValueCombiner FindCombiner(string combiner)
        {
            return CrossBindingSingletonCache.Instance?.ValueCombinerLookup.Find(combiner);
        }

        public IEnumerable<CrossBindingDescription> Parse(string text)
        {
            var parser = BindingParser;
            return Parse(text, parser);
        }

        public IEnumerable<CrossBindingDescription> Parse(string text, ICrossBindingParser parser)
        {
            CrossSerializableBindingSpecification specification;
            if (!parser.TryParseBindingSpecification(text, out specification))
            {
                CrossBindingLog.Instance?.LogError("Failed to parse binding description starting with {BindingText}",
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
            CrossSerializableBindingDescription description;
            var parser = BindingParser;
            if (!parser.TryParseBindingDescription(text, out description))
            {
                CrossBindingLog.Instance?.LogError("Failed to parse binding description starting with {BindingText}",
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
            string targetName, CrossSerializableBindingDescription description)
        {
            return new CrossBindingDescription
            {
                TargetName = targetName,
                Source = SourceStepDescriptionFrom(description),
                Mode = description.Mode,
            };
        }

        private CrossSourceStepDescription SourceStepDescriptionFrom(CrossSerializableBindingDescription description)
        {
            if (description.Path != null)
            {
                return new CrossPathSourceStepDescription()
                {
                    SourcePropertyPath = description.Path,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    FallbackValue = description.FallbackValue
                };
            }

            if (description.Literal != null)
            {
                throw new NotImplementedException();

                //var literal = description.Literal;
                //if (literal == CrossTibetBindingParser.LiteralNull)
                //    literal = null;

                //return new CrossLiteralSourceStepDescription()
                //{
                //    Literal = literal,
                //    Converter = FindConverter(description.Converter),
                //    ConverterParameter = description.ConverterParameter,
                //    FallbackValue = description.FallbackValue
                //};
            }

            if (description.Function != null)
            {
                // first look for a combiner with the name
                var combiner = FindCombiner(description.Function);
                if (combiner != null)
                {
                    return new CrossCombinerSourceStepDescription()
                    {
                        Combiner = combiner,
                        InnerSteps = description.Sources == null
                            ? new List<CrossSourceStepDescription>() :
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
                        CrossBindingLog.Instance?.LogError("Failed to find combiner or converter for {FunctionName}",
                            description.Function);
                    }

                    if (description.Sources == null || description.Sources.Count == 0)
                    {
                        CrossBindingLog.Instance?.LogError("Value Converter {FunctionName} supplied with no source",
                            description.Function);
                        return new CrossLiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else if (description.Sources.Count > 2)
                    {
                        CrossBindingLog.Instance?.LogError(
                            "Value Converter {FunctionName} supplied with too many parameters - {ParameterCount}",
                            description.Function, description.Sources.Count);
                        return new CrossLiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else
                    {
                        throw new NotImplementedException();

                        //return new CrossCombinerSourceStepDescription()
                        //{
                        //    Combiner = new CrossValueConverterValueCombiner(converter),
                        //    InnerSteps = description.Sources.Select(source => SourceStepDescriptionFrom(source)).ToList(),
                        //    Converter = FindConverter(description.Converter),
                        //    ConverterParameter = description.ConverterParameter,
                        //    FallbackValue = description.FallbackValue
                        //};
                    }
                }
            }

            // this probably suggests that the path is the entire source object
            return new CrossPathSourceStepDescription()
            {
                SourcePropertyPath = null,
                Converter = FindConverter(description.Converter),
                ConverterParameter = description.ConverterParameter,
                FallbackValue = description.FallbackValue
            };
        }
    }
}
