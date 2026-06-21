using Microsoft.Extensions.DependencyInjection;
using Nivaes.IoC;

namespace Nivaes.App.Cross;

// this class is not perfect OO and it gets in the way of testing
// however, it is here for speed - to help avoid obscene numbers of IPlatformApplication.Current!.Services.GetRequiredService<T> calls during binding
public class CrossBindingSingletonCache
    : ICrossBindingSingletonCache
{
    //public static ICrossBindingSingletonCache Initialize()
    //{
    //    //if (Instance != null)
    //    //    throw new CrossException("You should only initialize MvxBindingSingletonCache once");

    //    var instance = new CrossBindingSingletonCache();
    //    return instance;
    //}

    private ICrossAutoValueConverters? _autoValueConverters;
    private ICrossBindingDescriptionParser? _bindingDescriptionParser;
    private ICrossSourceBindingFactory? _sourceBindingFactory;
    private ICrossTargetBindingFactory? _targetBindingFactory;
    private ICrossLanguageBindingParser? _languageParser;
    private ICrossPropertyExpressionParser? _propertyExpressionParser;
    private ICrossValueConverterLookup? _valueConverterLookup;
    private ICrossBindingNameLookup? _defaultBindingName;
    private ICrossBinder? _binder;
    private ICrossSourceStepFactory? _sourceStepFactory;
    private ICrossValueCombinerLookup? _valueCombinerLookup;
    private ICrossMainThreadAsyncDispatcher? _mainThreadDispatcher;

    public ICrossAutoValueConverters AutoValueConverters
    {
        get
        {
            _autoValueConverters = _autoValueConverters ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossAutoValueConverters>();
            return _autoValueConverters;
        }
    }

    public ICrossBindingDescriptionParser BindingDescriptionParser
    {
        get
        {
            _bindingDescriptionParser = _bindingDescriptionParser ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingDescriptionParser>();
            return _bindingDescriptionParser;
        }
    }

    public ICrossLanguageBindingParser LanguageParser
    {
        get
        {
            _languageParser = _languageParser ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossLanguageBindingParser>();
            return _languageParser;
        }
    }

    public ICrossPropertyExpressionParser PropertyExpressionParser
    {
        get
        {
            _propertyExpressionParser = _propertyExpressionParser ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossPropertyExpressionParser>();
            return _propertyExpressionParser;
        }
    }

    public ICrossValueConverterLookup ValueConverterLookup
    {
        get
        {
            _valueConverterLookup = _valueConverterLookup ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossValueConverterLookup>();
            return _valueConverterLookup;
        }
    }

    public ICrossValueCombinerLookup ValueCombinerLookup
    {
        get
        {
            _valueCombinerLookup = _valueCombinerLookup ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossValueCombinerLookup>();
            return _valueCombinerLookup;
        }
    }

    public ICrossBindingNameLookup DefaultBindingNameLookup
    {
        get
        {
            _defaultBindingName = _defaultBindingName ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingNameLookup>();
            return _defaultBindingName;
        }
    }

    public ICrossBinder Binder
    {
        get
        {
            _binder = _binder ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossBinder>();
            return _binder;
        }
    }

    public ICrossSourceBindingFactory SourceBindingFactory
    {
        get
        {
            _sourceBindingFactory = _sourceBindingFactory ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossSourceBindingFactory>();
            return _sourceBindingFactory;
        }
    }

    public ICrossTargetBindingFactory TargetBindingFactory
    {
        get
        {
            _targetBindingFactory = _targetBindingFactory ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossTargetBindingFactory>();
            return _targetBindingFactory;
        }
    }

    public ICrossSourceStepFactory SourceStepFactory
    {
        get
        {
            _sourceStepFactory = _sourceStepFactory ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossSourceStepFactory>();
            return _sourceStepFactory;
        }
    }

    public ICrossMainThreadAsyncDispatcher MainThreadDispatcher
    {
        get
        {
            _mainThreadDispatcher = _mainThreadDispatcher ?? IPlatformApplication.Current!.Services.GetRequiredService<ICrossMainThreadAsyncDispatcher>();
            return _mainThreadDispatcher;
        }
    }
}
