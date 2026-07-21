using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

// this class is not perfect OO and it gets in the way of testing
// however, it is here for speed - to help avoid obscene numbers of IPlatformApplication.Current!.Services.GetRequiredService<T> calls during binding
public class CrossBindingSingletonCache
    : ICrossBindingSingletonCache
{
    private ICrossBindingDescriptionParser? _bindingDescriptionParser;
    private ICrossSourceBindingFactory? _sourceBindingFactory;
    private ICrossTargetBindingFactory? _targetBindingFactory;
    private ICrossLanguageBindingParser? _languageParser;
    private ICrossPropertyExpressionParser? _propertyExpressionParser;


    private ICrossBindingNameLookup? _defaultBindingName;
    private ICrossBinder? _binder;
    private ICrossSourceStepFactory? _sourceStepFactory;


    private ICrossMainThreadDispatcher? _mainThreadDispatcher;

    public ICrossBindingDescriptionParser BindingDescriptionParser
    {
        get
        {
            _bindingDescriptionParser = _bindingDescriptionParser ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossBindingDescriptionParser>();
            return _bindingDescriptionParser;
        }
    }

    public ICrossLanguageBindingParser LanguageParser
    {
        get
        {
            _languageParser = _languageParser ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossLanguageBindingParser>();
            return _languageParser;
        }
    }

    public ICrossPropertyExpressionParser PropertyExpressionParser
    {
        get
        {
            _propertyExpressionParser = _propertyExpressionParser ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossPropertyExpressionParser>();
            return _propertyExpressionParser;
        }
    }

    public ICrossBindingNameLookup DefaultBindingNameLookup
    {
        get
        {
            _defaultBindingName = _defaultBindingName ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossBindingNameLookup>();
            return _defaultBindingName;
        }
    }

    public ICrossBinder Binder
    {
        get
        {
            _binder = _binder ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossBinder>();
            return _binder;
        }
    }

    public ICrossSourceBindingFactory SourceBindingFactory
    {
        get
        {
            _sourceBindingFactory = _sourceBindingFactory ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossSourceBindingFactory>();
            return _sourceBindingFactory;
        }
    }

    public ICrossSourceStepFactory SourceStepFactory
    {
        get
        {
            _sourceStepFactory = _sourceStepFactory ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossSourceStepFactory>();
            return _sourceStepFactory;
        }
    }

    public ICrossMainThreadDispatcher MainThreadDispatcher
    {
        get
        {
            _mainThreadDispatcher = _mainThreadDispatcher ?? IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossMainThreadDispatcher>();
            return _mainThreadDispatcher;
        }
    }
}
