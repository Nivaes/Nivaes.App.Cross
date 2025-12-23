namespace Nivaes.App.Cross
{
    using MvvmCross;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.ExpressionParse;
    using Nivaes.App.Cross;

    // this class is not perfect OO and it gets in the way of testing
    // however, it is here for speed - to help avoid obscene numbers of Mvx.IoCProvider.Resolve<T> calls during binding
    public class CrossBindingSingletonCache
        : CrossSingleton<ICrossBindingSingletonCache>, ICrossBindingSingletonCache
    {
        public static ICrossBindingSingletonCache Initialize()
        {
            if (Instance != null)
                throw new CrossException("You should only initialize MvxBindingSingletonCache once");

            var instance = new CrossBindingSingletonCache();
            return instance;
        }

        private ICrossAutoValueConverters _autoValueConverters;
        private ICrossBindingDescriptionParser _bindingDescriptionParser;
        private ICrossSourceBindingFactory _sourceBindingFactory;
        private ICrossTargetBindingFactory _targetBindingFactory;
        private ICrossLanguageBindingParser _languageParser;
        private ICrossPropertyExpressionParser _propertyExpressionParser;
        private ICrossValueConverterLookup _valueConverterLookup;
        private ICrossBindingNameLookup _defaultBindingName;
        private ICrossBinder _binder;
        private ICrossSourceStepFactory _sourceStepFactory;
        private ICrossValueCombinerLookup _valueCombinerLookup;
        private ICrossMainThreadAsyncDispatcher _mainThreadDispatcher;

        public ICrossAutoValueConverters AutoValueConverters
        {
            get
            {
                _autoValueConverters = _autoValueConverters ?? Mvx.IoCProvider.Resolve<ICrossAutoValueConverters>();
                return _autoValueConverters;
            }
        }

        public ICrossBindingDescriptionParser BindingDescriptionParser
        {
            get
            {
                _bindingDescriptionParser = _bindingDescriptionParser ?? Mvx.IoCProvider.Resolve<ICrossBindingDescriptionParser>();
                return _bindingDescriptionParser;
            }
        }

        public ICrossLanguageBindingParser LanguageParser
        {
            get
            {
                _languageParser = _languageParser ?? Mvx.IoCProvider.Resolve<ICrossLanguageBindingParser>();
                return _languageParser;
            }
        }

        public ICrossPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                _propertyExpressionParser = _propertyExpressionParser ?? Mvx.IoCProvider.Resolve<ICrossPropertyExpressionParser>();
                return _propertyExpressionParser;
            }
        }

        public ICrossValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? Mvx.IoCProvider.Resolve<ICrossValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        public ICrossValueCombinerLookup ValueCombinerLookup
        {
            get
            {
                _valueCombinerLookup = _valueCombinerLookup ?? Mvx.IoCProvider.Resolve<ICrossValueCombinerLookup>();
                return _valueCombinerLookup;
            }
        }

        public ICrossBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                _defaultBindingName = _defaultBindingName ?? Mvx.IoCProvider.Resolve<ICrossBindingNameLookup>();
                return _defaultBindingName;
            }
        }

        public ICrossBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.IoCProvider.Resolve<ICrossBinder>();
                return _binder;
            }
        }

        public ICrossSourceBindingFactory SourceBindingFactory
        {
            get
            {
                _sourceBindingFactory = _sourceBindingFactory ?? Mvx.IoCProvider.Resolve<ICrossSourceBindingFactory>();
                return _sourceBindingFactory;
            }
        }

        public ICrossTargetBindingFactory TargetBindingFactory
        {
            get
            {
                _targetBindingFactory = _targetBindingFactory ?? Mvx.IoCProvider.Resolve<ICrossTargetBindingFactory>();
                return _targetBindingFactory;
            }
        }

        public ICrossSourceStepFactory SourceStepFactory
        {
            get
            {
                _sourceStepFactory = _sourceStepFactory ?? Mvx.IoCProvider.Resolve<ICrossSourceStepFactory>();
                return _sourceStepFactory;
            }
        }

        public ICrossMainThreadAsyncDispatcher MainThreadDispatcher
        {
            get
            {
                _mainThreadDispatcher = _mainThreadDispatcher ?? Mvx.IoCProvider.Resolve<ICrossMainThreadAsyncDispatcher>();
                return _mainThreadDispatcher;
            }
        }
    }
}
