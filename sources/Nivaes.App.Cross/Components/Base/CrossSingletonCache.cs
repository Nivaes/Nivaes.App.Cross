using System;

namespace Nivaes.App.Cross
{
    [Obsolete("¿Hace falta?")]
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
                throw new CrossException("You should only initialize CrossBindingSingletonCache once");

            return new CrossSingletonCache();
        }

        private CrossSingletonCache()
        {
        }

        public ICrossInpcInterceptor? InpcInterceptor
        {
            get
            {
                throw new NotImplementedException();
                //if (_inpcInterceptorResolveAttempted)
                //    return _inpcInterceptor;

                //Cross.IoCProvider?.TryResolve(out _inpcInterceptor);
                //_inpcInterceptorResolveAttempted = true;
                //return _inpcInterceptor;
            }
        }

        public ICrossStringToTypeParser? Parser
        {
            get
            {
                throw new NotImplementedException();
                //_parser ??= Cross.IoCProvider?.Resolve<ICrossStringToTypeParser>();
                //return _parser;
            }
        }

        public ICrossSettings? Settings
        {
            get
            {
                throw new NotImplementedException();

                //_settings ??= Cross.IoCProvider?.Resolve<ICrossSettings>();
                //return _settings;
            }
        }
    }
}