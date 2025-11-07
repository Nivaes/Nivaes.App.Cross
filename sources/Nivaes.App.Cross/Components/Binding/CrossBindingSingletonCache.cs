namespace Nivaes.App.Cross
{
    // this class is not perfect OO and it gets in the way of testing
    // however, it is here for speed - to help avoid obscene numbers of Cross.IoCProvider.Resolve<T> calls during binding
    [Obsolete("Quitar IoC de Cross")]
    public class CrossBindingSingletonCache
        : CrossSingleton<ICrossBindingSingletonCache>, ICrossBindingSingletonCache
    {
        public static ICrossBindingSingletonCache Initialize()
        {
            throw new NotImplementedException();
            //if (Instance != null)
            //    throw new CrossException("You should only initialize CrossBindingSingletonCache once");

            //var instance = new CrossBindingSingletonCache();
            //return instance;
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
                throw new NotImplementedException();
                //_autoValueConverters = _autoValueConverters ?? Cross.IoCProvider.Resolve<ICrossAutoValueConverters>();
                //return _autoValueConverters;
            }
        }

        public ICrossBindingDescriptionParser BindingDescriptionParser
        {
            get
            {
                throw new NotImplementedException();

                //_bindingDescriptionParser = _bindingDescriptionParser ?? Cross.IoCProvider.Resolve<ICrossBindingDescriptionParser>();
                //return _bindingDescriptionParser;
            }
        }

        public ICrossLanguageBindingParser LanguageParser
        {
            get
            {
                throw new NotImplementedException();
                //_languageParser = _languageParser ?? Cross.IoCProvider.Resolve<ICrossLanguageBindingParser>();
                //return _languageParser;
            }
        }

        public ICrossPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                throw new NotImplementedException();
                //_propertyExpressionParser = _propertyExpressionParser ?? Cross.IoCProvider.Resolve<ICrossPropertyExpressionParser>();
                //return _propertyExpressionParser;
            }
        }

        public ICrossValueConverterLookup ValueConverterLookup
        {
            get
            {
                throw new NotImplementedException();

                //_valueConverterLookup = _valueConverterLookup ?? Cross.IoCProvider.Resolve<ICrossValueConverterLookup>();
                //return _valueConverterLookup;
            }
        }

        public ICrossValueCombinerLookup ValueCombinerLookup
        {
            get
            {
                throw new NotImplementedException();

                //_valueCombinerLookup = _valueCombinerLookup ?? Cross.IoCProvider.Resolve<ICrossValueCombinerLookup>();
                //return _valueCombinerLookup;
            }
        }

        public ICrossBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                throw new NotImplementedException();

                //_defaultBindingName = _defaultBindingName ?? Cross.IoCProvider.Resolve<ICrossBindingNameLookup>();
                //return _defaultBindingName;
            }
        }

        public ICrossBinder Binder
        {
            get
            {
                throw new NotImplementedException();

                //_binder = _binder ?? Cross.IoCProvider.Resolve<ICrossBinder>();
                //return _binder;
            }
        }

        public ICrossSourceBindingFactory SourceBindingFactory
        {
            get
            {
                throw new NotImplementedException();

                //_sourceBindingFactory = _sourceBindingFactory ?? Cross.IoCProvider.Resolve<ICrossSourceBindingFactory>();
                //return _sourceBindingFactory;
            }
        }

        public ICrossTargetBindingFactory TargetBindingFactory
        {
            get
            {
                throw new NotImplementedException();

                //_targetBindingFactory = _targetBindingFactory ?? Cross.IoCProvider.Resolve<ICrossTargetBindingFactory>();
                //return _targetBindingFactory;
            }
        }

        public ICrossSourceStepFactory SourceStepFactory
        {
            get
            {
                throw new NotImplementedException();

                //_sourceStepFactory = _sourceStepFactory ?? Cross.IoCProvider.Resolve<ICrossSourceStepFactory>();
                //return _sourceStepFactory;
            }
        }

        public ICrossMainThreadAsyncDispatcher MainThreadDispatcher
        {
            get
            {
                throw new NotImplementedException();

                //_mainThreadDispatcher = _mainThreadDispatcher ?? Cross.IoCProvider.Resolve<ICrossMainThreadAsyncDispatcher>();
                //return _mainThreadDispatcher;
            }
        }
    }
}
