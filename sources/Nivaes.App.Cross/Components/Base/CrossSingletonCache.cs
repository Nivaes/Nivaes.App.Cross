namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.DependencyInjection;

    public sealed class CrossSingletonCache
    : CrossSingleton<ICrossSingletonCache>, ICrossSingletonCache
    {
        private bool _inpcInterceptorResolveAttempted;
        private ICrossInpcInterceptor? _inpcInterceptor;
        private ICrossStringToTypeParser? _parser;
        private ICrossSettings? _settings;

        public static CrossSingletonCache Initialize()
        {
            if (Instance != null)
                throw new CrossException("You should only initialize MvxBindingSingletonCache once");

            return new CrossSingletonCache();
        }

        private CrossSingletonCache()
        {
        }

        public ICrossInpcInterceptor? InpcInterceptor
        {
            get
            {
                if (_inpcInterceptorResolveAttempted)
                    return _inpcInterceptor;

                _inpcInterceptor = IPlatformApplication.Current!.Services.GetRequiredService<ICrossInpcInterceptor>();

                _inpcInterceptorResolveAttempted = true;
                return _inpcInterceptor;
            }
        }

        public ICrossStringToTypeParser? Parser
        {
            get
            {
                _parser ??= IPlatformApplication.Current!.Services.GetRequiredService<ICrossStringToTypeParser>();
                return _parser;
            }
        }

        public ICrossSettings? Settings
        {
            get
            {
                _settings ??= IPlatformApplication.Current!.Services.GetRequiredService<ICrossSettings>();
                return _settings;
            }
        }
    }
}
