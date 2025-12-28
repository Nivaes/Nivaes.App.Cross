namespace Nivaes.App.Cross
{
    using Nivaes.IoC;

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

                Mvx.IoCProvider?.TryResolve(out _inpcInterceptor);
                //_inpcInterceptor = Mvx.IoCProvider?.Resolve<ICrossInpcInterceptor>();
                _inpcInterceptorResolveAttempted = true;
                return _inpcInterceptor;
            }
        }

        public ICrossStringToTypeParser? Parser
        {
            get
            {
                _parser ??= Mvx.IoCProvider?.Resolve<ICrossStringToTypeParser>();
                return _parser;
            }
        }

        public ICrossSettings? Settings
        {
            get
            {
                _settings ??= Mvx.IoCProvider?.Resolve<ICrossSettings>();
                return _settings;
            }
        }
    }
}
