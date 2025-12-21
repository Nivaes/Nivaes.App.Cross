namespace Nivaes.App.Cross
{
    using MvvmCross;
    using MvvmCross.Base;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using MvvmCross.ViewModels;

    public sealed class CrossSingletonCache
    : MvxSingleton<ICrossSingletonCache>, ICrossSingletonCache
    {
        private bool _inpcInterceptorResolveAttempted;
        private ICrossInpcInterceptor? _inpcInterceptor;
        private IMvxStringToTypeParser? _parser;
        private IMvxSettings? _settings;

        public static CrossSingletonCache Initialize()
        {
            if (Instance != null)
                throw new MvxException("You should only initialize MvxBindingSingletonCache once");

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
                _inpcInterceptorResolveAttempted = true;
                return _inpcInterceptor;
            }
        }

        public IMvxStringToTypeParser? Parser
        {
            get
            {
                _parser ??= Mvx.IoCProvider?.Resolve<IMvxStringToTypeParser>();
                return _parser;
            }
        }

        public IMvxSettings? Settings
        {
            get
            {
                _settings ??= Mvx.IoCProvider?.Resolve<IMvxSettings>();
                return _settings;
            }
        }
    }
}
