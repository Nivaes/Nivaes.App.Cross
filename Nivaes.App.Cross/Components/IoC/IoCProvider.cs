// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// <para>Singleton IoC Provider.</para>
    /// <para>Delegates to the <see cref="MvxIoCContainer"/> implementation</para>
    /// </summary>
    public class IoCProvider
        : IIoCProvider
    {
        #region Singleton
        private static readonly Lazy<IIoCProvider> mProviderLazy = new Lazy<IIoCProvider>(Initialize);

        public static IIoCProvider Provider => mProviderLazy.Value;
        public static bool IsValueCreated => mProviderLazy.IsValueCreated;

        private readonly IoCContainer mContainer;

        protected IoCProvider(IMvxIocOptions? options)
        {
            mContainer = new IoCContainer(options);
        }

        private static readonly TaskCompletionSource<IIoCProvider> mInitializeTaskCompletation
            = new TaskCompletionSource<IIoCProvider>();

        private static IIoCProvider Initialize()
        {
            return mInitializeTaskCompletation.Task.GetAwaiter().GetResult();
        }

        public static IIoCProvider Initialize(IMvxIocOptions? options = null)
        {
            var privider = new IoCProvider(options);

            mInitializeTaskCompletation.SetResult(privider);

            return privider;
        }
        #endregion

        #region IMvxIoCProvider
        public bool CanResolve<T>()
            where T : class
        {
            return mContainer.CanResolve<T>();
        }

        public bool CanResolve(Type t)
        {
            return mContainer.CanResolve(t);
        }

        public bool TryResolve<T>(out T resolved)
            where T : class
        {
            return mContainer.TryResolve<T>(out resolved);
        }

        public bool TryResolve(Type type, out object? resolved)
        {
            return mContainer.TryResolve(type, out resolved);
        }

        public T Resolve<T>()
            where T : class
        {
            return mContainer.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return mContainer.Resolve(t);
        }

        public T GetSingleton<T>()
            where T : class
        {
            return mContainer.GetSingleton<T>();
        }

        public object GetSingleton(Type t)
        {
            return mContainer.GetSingleton(t);
        }

        public T Create<T>()
            where T : class
        {
            return mContainer.Create<T>();
        }

        public object? Create(Type t)
        {
            return mContainer.Create(t);
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            mContainer.RegisterType<TInterface, TToConstruct>();
        }

        public void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            mContainer.RegisterType(constructor);
        }

        public void RegisterType(Type t, Func<object> constructor)
        {
            mContainer.RegisterType(t, constructor);
        }

        public void RegisterType(Type interfaceType, Type constructType)
        {
            mContainer.RegisterType(interfaceType, constructType);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            mContainer.RegisterSingleton(theObject);
        }

        public void RegisterSingleton(Type interfaceType, object theObject)
        {
            mContainer.RegisterSingleton(interfaceType, theObject);
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            mContainer.RegisterSingleton(theConstructor);
        }

        public void RegisterSingleton(Type interfaceType, Func<object> theConstructor)
        {
            mContainer.RegisterSingleton(interfaceType, theConstructor);
        }

        public T IoCConstruct<T>()
            where T : class
        {
            return mContainer.IoCConstruct<T>();
        }

        public virtual object IoCConstruct(Type type)
        {
            return mContainer.IoCConstruct(type);
        }

        public T IoCConstruct<T>(IDictionary<string, object> arguments) where T : class
        {
            return mContainer.IoCConstruct<T>(arguments);
        }

        public T IoCConstruct<T>(params object[] arguments) where T : class
        {
            return mContainer.IoCConstruct<T>(arguments);
        }

        public T IoCConstruct<T>(object arguments) where T : class
        {
            return mContainer.IoCConstruct<T>(arguments);
        }

        public object IoCConstruct(Type type, IDictionary<string, object> arguments = null)
        {
            return mContainer.IoCConstruct(type, arguments);
        }

        public object IoCConstruct(Type type, object arguments)
        {
            return mContainer.IoCConstruct(type, arguments);
        }

        public object IoCConstruct(Type type, params object[] arguments)
        {
            return mContainer.IoCConstruct(type, arguments);
        }

        public void CallbackWhenRegistered<T>(Action action)
        {
            mContainer.CallbackWhenRegistered<T>(action);
        }

        public void CallbackWhenRegistered(Type type, Action action)
        {
            mContainer.CallbackWhenRegistered(type, action);
        }

        public void CleanAllResolvers()
        {
            mContainer.CleanAllResolvers();
        }

        public IIoCProvider CreateChildContainer()
        {
            return mContainer.CreateChildContainer();
        }
        #endregion
    }
}
