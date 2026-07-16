namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using Microsoft.Extensions.DependencyInjection;

    public class CrossBaseFluentBindingDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>
        : CrossApplicableTo<TTarget>, ICrossBaseFluentBindingDescription
        where TTarget : class
    {
        private readonly TTarget? _target;
        private readonly ICrossBindingContextOwner? _bindingContextOwner;

        private readonly CrossBindingDescription _bindingDescription = new CrossBindingDescription();
        private readonly CrossSourceStepDescription _sourceStepDescription = new CrossSourceStepDescription();
        private ISourceSpec _sourceSpec;

        public interface ISourceSpec
        {
            CrossSourceStepDescription CreateSourceStep(CrossSourceStepDescription inputs);
        }

        public class KnownPathSourceSpec
            : ISourceSpec
        {
            private readonly string _knownSourcePath;

            public KnownPathSourceSpec(string knownSourcePath)
            {
                _knownSourcePath = knownSourcePath;
            }

            public CrossSourceStepDescription CreateSourceStep(CrossSourceStepDescription inputs)
            {
                return new CrossPathSourceStepDescription()
                {
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    SourcePropertyPath = _knownSourcePath
                };
            }
        }

        public class FreeTextSourceSpec
            : ISourceSpec
        {
            private readonly string _freeText;

            public FreeTextSourceSpec(string freeText)
            {
                _freeText = freeText;
            }

            public CrossSourceStepDescription CreateSourceStep(CrossSourceStepDescription inputs)
            {
                var parser = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossBindingDescriptionParser>();
                var parsedDescription = parser?.ParseSingle(_freeText);

                if (inputs.Converter == null
                    && inputs.FallbackValue == null)
                {
                    return parsedDescription!.Source;
                }

                if (parsedDescription!.Source.Converter == null
                    && parsedDescription.Source.FallbackValue == null)
                {
                    var parsedStep = parsedDescription.Source;
                    parsedStep.Converter = inputs.Converter;
                    parsedStep.ConverterParameter = inputs.ConverterParameter;
                    parsedStep.FallbackValue = inputs.FallbackValue;
                    return parsedStep;
                }

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, parsedDescription.Source);
            }
        }

        public class FullySourceSpec
            : ISourceSpec
        {
            private readonly CrossSourceStepDescription _sourceStepDescription;

            public FullySourceSpec(CrossSourceStepDescription sourceStepDescription)
            {
                _sourceStepDescription = sourceStepDescription;
            }

            public CrossSourceStepDescription CreateSourceStep(CrossSourceStepDescription inputs)
            {
                if (inputs.Converter == null || inputs.FallbackValue == null)
                {
                    return _sourceStepDescription;
                }

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, _sourceStepDescription);
            }
        }

        public class CombinerSourceSpec
            : ISourceSpec
        {
            private readonly bool _useParser;
            private readonly string[] _properties;
            private readonly ICrossValueCombiner _combiner;

            public CombinerSourceSpec(ICrossValueCombiner combiner, string[] properties, bool useParser)
            {
                _combiner = combiner;
                _useParser = useParser;
                _properties = properties;
            }

            public CrossSourceStepDescription CreateSourceStep(CrossSourceStepDescription inputs)
            {
                var parser = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossBindingDescriptionParser>();
                var innerSteps = _useParser ?
                    _properties.Select(p => parser.ParseSingle(p).Source) :
                    _properties.Select(p => new CrossPathSourceStepDescription { SourcePropertyPath = p });

                return new CrossCombinerSourceStepDescription
                {
                    Combiner = _combiner,
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    InnerSteps = [.. innerSteps]
                };
            }
        }

        public static class SourceSpecHelpers
        {
            public static CrossSourceStepDescription WrapInsideSingleCombiner(CrossSourceStepDescription inputs,
                                                                        CrossSourceStepDescription sourceStepDescription)
            {
                return new CrossCombinerSourceStepDescription()
                {
                    Combiner = new CrossSingleValueCombiner(),
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    InnerSteps = [sourceStepDescription]
                };
            }
        }

        protected object ClearBindingKey { get; set; }

        object ICrossBaseFluentBindingDescription.ClearBindingKey
        {
            get => ClearBindingKey;
            set => ClearBindingKey = value;
        }

        protected CrossBindingDescription BindingDescription => _bindingDescription;

        protected CrossSourceStepDescription SourceStepDescription => _sourceStepDescription;

        protected void SetFreeTextPropertyPath(string sourcePropertyPath)
        {
            if (_sourceSpec != null)
                throw new AppException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new FreeTextSourceSpec(sourcePropertyPath);
        }

        protected void SetKnownTextPropertyPath(string sourcePropertyPath)
        {
            if (_sourceSpec != null)
                throw new AppException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new KnownPathSourceSpec(sourcePropertyPath);
        }

        protected void SetCombiner(ICrossValueCombiner combiner, string[] properties, bool useParser)
        {
            if (_sourceSpec != null)
                throw new AppException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new CombinerSourceSpec(combiner, properties, useParser);
        }

        protected void SourceOverwrite(CrossBindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new AppException("You cannot set the source path of a Fluent binding more than once");

            _bindingDescription.Mode = bindingDescription.Mode;
            _bindingDescription.TargetName = bindingDescription.TargetName;

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        protected void FullOverwrite(CrossBindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new AppException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        public CrossBaseFluentBindingDescription(ICrossBindingContextOwner? bindingContextOwner, TTarget? target)
        {
            _bindingContextOwner = bindingContextOwner;
            _target = target;
        }

        protected static string TargetPropertyName(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var parser = Singleton<CrossBindingSingletonCache>.Instance.PropertyExpressionParser;
            var targetPropertyName = parser.Parse(targetPropertyPath).Print();
            return targetPropertyName;
        }

        protected static string SourcePropertyPath<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var parser = Singleton<CrossBindingSingletonCache>.Instance.PropertyExpressionParser;
            var sourcePropertyPath = parser.Parse(sourceProperty).Print();
            return sourcePropertyPath;
        }

        protected static ICrossValueConverter ValueConverterFromName(string converterName)
        {
            //var converter = Singleton<CrossBindingSingletonCache>.Instance.ValueConverterLookup.Find(converterName);
            //return converter;

            var converter = Singleton<NameConvertersKeyContainerManager>.Instance.GetValue(converterName);
            return converter!;
        }

        protected CrossBindingDescription CreateBindingDescription()
        {
            EnsureTargetNameSet();

            CrossSourceStepDescription source;
            if (_sourceSpec == null)
            {
                source = new CrossPathSourceStepDescription()
                {
                    Converter = _sourceStepDescription.Converter,
                    ConverterParameter = _sourceStepDescription.ConverterParameter,
                    FallbackValue = _sourceStepDescription.FallbackValue
                };
            }
            else
            {
                source = _sourceSpec.CreateSourceStep(_sourceStepDescription);
            }

            var toReturn = new CrossBindingDescription()
            {
                Mode = BindingDescription.Mode,
                TargetName = BindingDescription.TargetName,
                Source = source
            };

            return toReturn;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public override void Apply()
        {
            var bindingDescription = CreateBindingDescription();
            _bindingContextOwner.AddBinding(_target, bindingDescription, ClearBindingKey);
            base.Apply();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public override void ApplyTo(TTarget what)
        {
            var bindingDescription = CreateBindingDescription();
            _bindingContextOwner.AddBinding(what, bindingDescription, ClearBindingKey);
            base.ApplyTo(what);
        }

        protected void EnsureTargetNameSet()
        {
            if (!string.IsNullOrEmpty(BindingDescription.TargetName))
                return;

            var defaultTargetName =
                Singleton<CrossBindingSingletonCache>.Instance?.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));

            if (string.IsNullOrEmpty(defaultTargetName))
            {
                throw new AppException(
                    "Default Target Name, could not be found for Target: {0}. Did you register a default?",
                    typeof(TTarget));
            }

            BindingDescription.TargetName = defaultTargetName;
        }
    }
}
