// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross.Logging;

    internal class MvxIoCContainer
        : IMvxIoCProvider
    {
        private readonly Dictionary<Type, IResolver> mResolvers = new Dictionary<Type, IResolver>();
        private readonly Dictionary<Type, List<Action>> mWaiters = new Dictionary<Type, List<Action>>();
        private readonly Dictionary<Type, bool> mCircularTypeDetection = new Dictionary<Type, bool>();
        private readonly object mLockObject = new object();
        private readonly IMvxIocOptions mOptions;
        private readonly IMvxPropertyInjector? mPropertyInjector;
        private readonly IMvxIoCProvider? mParentProvider;

        protected IMvxIocOptions Options => mOptions;

        public MvxIoCContainer(IMvxIocOptions? options, IMvxIoCProvider? parentProvider = null)
        {
            mOptions = options ?? new MvxIocOptions();
            if (mOptions.PropertyInjectorType != null)
            {
                mPropertyInjector = Activator.CreateInstance(mOptions.PropertyInjectorType) as IMvxPropertyInjector;
            }
            if (mPropertyInjector != null)
            {
                RegisterSingleton(typeof(IMvxPropertyInjector), mPropertyInjector);
            }
            if (parentProvider != null)
            {
                mParentProvider = parentProvider;
            }
        }

        public MvxIoCContainer(IMvxIoCProvider parentProvider)
            : this(null, parentProvider)
        {
            if (parentProvider == null)
            {
                throw new ArgumentNullException(nameof(parentProvider), "Provide a parent ioc provider to this constructor");
            }
        }

        public interface IResolver
        {
            object Resolve();

            ResolverType ResolveType { get; }

            void SetGenericTypeParameters(Type[] genericTypeParameters);
        }

        public class ConstructingResolver : IResolver
        {
            private readonly Type _type;
            private readonly IMvxIoCProvider _parent;

            public ConstructingResolver(Type type, IMvxIoCProvider parent)
            {
                _type = type;
                _parent = parent;
            }

            public object Resolve()
            {
                return _parent.IoCConstruct(_type);
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public class FuncConstructingResolver : IResolver
        {
            private readonly Func<object> _constructor;

            public FuncConstructingResolver(Func<object> constructor)
            {
                _constructor = constructor;
            }

            public object Resolve()
            {
                return _constructor();
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public class SingletonResolver : IResolver
        {
            private readonly object mTheObject;

            public SingletonResolver(object theObject)
            {
                mTheObject = theObject;
            }

            public object Resolve()
            {
                return mTheObject;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.Singleton;
        }

        public class ConstructingSingletonResolver : IResolver
        {
            private readonly object mSyncObject = new object();
            private readonly Func<object> mConstructor;
            private object? mTheObject;

            public ConstructingSingletonResolver(Func<object> theConstructor)
            {
                mConstructor = theConstructor;
            }

            public object Resolve()
            {
                if (mTheObject != null)
                    return mTheObject;

                lock (mSyncObject)
                {
                    if (mTheObject == null)
                        mTheObject = mConstructor();
                }

                return mTheObject;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.Singleton;
        }

        public class ConstructingOpenGenericResolver : IResolver
        {
            private readonly Type _genericTypeDefinition;
            private readonly IMvxIoCProvider _parent;

            private Type[] _genericTypeParameters;

            public ConstructingOpenGenericResolver(Type genericTypeDefinition, IMvxIoCProvider parent)
            {
                _genericTypeDefinition = genericTypeDefinition;
                _parent = parent;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                _genericTypeParameters = genericTypeParameters;
            }

            public object Resolve()
            {
                return _parent.IoCConstruct(_genericTypeDefinition.MakeGenericType(_genericTypeParameters));
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public bool CanResolve<T>()
            where T : class
        {
            return CanResolve(typeof(T));
        }

        public bool CanResolve(Type t)
        {
            lock (mLockObject)
            {
                if (mResolvers.ContainsKey(t))
                {
                    return true;
                }
                if (mParentProvider != null && mParentProvider.CanResolve(t))
                {
                    return true;
                }
                return false;
            }
        }

        public bool TryResolve<T>(out T resolved)
            where T : class
        {
            try
            {
                var toReturn = TryResolve(typeof(T), out object item);
                resolved = (T)item;
                return toReturn;
            }
            catch (MvxIoCResolveException)
            {
                resolved = (T)typeof(T).CreateDefault();
                return false;
            }
        }

        public bool TryResolve(Type type, out object resolved)
        {
            lock (mLockObject)
            {
                return InternalTryResolve(type, out resolved);
            }
        }

        public T Resolve<T>()
            where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type t)
        {
            if (t == null) throw new NullReferenceException(nameof(t));

            lock (mLockObject)
            {
                if (!InternalTryResolve(t, out object resolved))
                {
                    InternalTryResolve(t, out resolved);

                    throw new MvxIoCResolveException($"Failed to resolve type {t.FullName}");
                }
                return resolved;
            }
        }

        public T GetSingleton<T>()
            where T : class
        {
            return (T)GetSingleton(typeof(T));
        }

        public object GetSingleton(Type t)
        {
            if (t == null) throw new NullReferenceException(nameof(t));

            lock (mLockObject)
            {
                if (!InternalTryResolve(t, ResolverType.Singleton, out object resolved))
                {
                    throw new MvxIoCResolveException($"Failed to resolve type {t.FullName}");
                }
                return resolved;
            }
        }

        public T Create<T>()
            where T : class
        {
            return (T)Create(typeof(T));
        }

        public object Create(Type t)
        {
            if (t == null) throw new NullReferenceException(nameof(t));

            lock (mLockObject)
            {
                if (!InternalTryResolve(t, ResolverType.DynamicPerResolve, out object resolved))
                {
                    throw new MvxIoCResolveException($"Failed to resolve type {t.FullName}");
                }
                return resolved;
            }
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            RegisterType(typeof(TInterface), typeof(TToConstruct));
        }

        public void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var resolver = new FuncConstructingResolver(constructor);
            InternalSetResolver(typeof(TInterface), resolver);
        }

        public void RegisterType(Type t, Func<object> constructor)
        {
            var resolver = new FuncConstructingResolver(() =>
            {
                var ret = constructor();
                if (ret != null && !t.IsInstanceOfType(ret))
                    throw new MvxIoCResolveException($"Constructor failed to return a compatibly object for type {t.FullName}");

                return ret;
            });

            InternalSetResolver(t, resolver);
        }

        public void RegisterType(Type interfaceType, Type constructType)
        {
            IResolver resolver;
            if (interfaceType.GetTypeInfo().IsGenericTypeDefinition)
            {
                resolver = new ConstructingOpenGenericResolver(constructType, this);
            }
            else
            {
                resolver = new ConstructingResolver(constructType, this);
            }

            InternalSetResolver(interfaceType, resolver);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theObject);
        }

        public void RegisterSingleton(Type interfaceType, object theObject)
        {
            InternalSetResolver(interfaceType, new SingletonResolver(theObject));
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theConstructor);
        }

        public void RegisterSingleton(Type interfaceType, Func<object> theConstructor)
        {
            InternalSetResolver(interfaceType, new ConstructingSingletonResolver(theConstructor));
        }

        public object IoCConstruct(Type type)
        {
            return IoCConstruct(type, default(IDictionary<string, object>));
        }

        public T IoCConstruct<T>()
            where T : class
        {
            return (T)IoCConstruct(typeof(T));
        }

        public virtual object IoCConstruct(Type type, object arguments)
        {
            return IoCConstruct(type, arguments.ToPropertyDictionary());
        }

        public virtual T IoCConstruct<T>(IDictionary<string, object> arguments)
            where T : class
        {
            return (T)IoCConstruct(typeof(T), arguments);
        }

        public virtual T IoCConstruct<T>(object arguments)
            where T : class
        {
            return (T)IoCConstruct(typeof(T), arguments.ToPropertyDictionary());
        }

        public virtual T IoCConstruct<T>(params object[] arguments) where T : class
        {
            return (T)IoCConstruct(typeof(T), arguments);
        }

        public virtual object IoCConstruct(Type type, params object[] arguments)
        {
            var selectedConstructor = type.FindApplicableConstructor(arguments);

            if (selectedConstructor == null)
            {
                throw new MvxIoCResolveException($"Failed to find constructor for type { type.FullName } with arguments: { arguments.Select(x => x.GetType().Name + ", ") }");
            }

            var parameters = GetIoCParameterValues(type, selectedConstructor, arguments);
            return IoCConstruct(type, selectedConstructor, parameters.ToArray());
        }

        public virtual object IoCConstruct(Type type, IDictionary<string, object>? arguments)
        {
            var selectedConstructor = type.FindApplicableConstructor(arguments);

            if (selectedConstructor == null)
            {
                throw new MvxIoCResolveException("Failed to find constructor for type {0}", type.FullName);
            }

            var parameters = GetIoCParameterValues(type, selectedConstructor, arguments);
            return IoCConstruct(type, selectedConstructor, parameters.ToArray());
        }

        protected virtual object IoCConstruct(Type type, ConstructorInfo constructor, object[] arguments)
        {
            if (constructor == null) throw new NullReferenceException(nameof(constructor));

            object toReturn;
            try
            {
                toReturn = constructor.Invoke(arguments);
            }
            catch (TargetInvocationException invocation)
            {
                throw new MvxIoCResolveException(invocation, "Failed to construct {0}", type.Name);
            }

            try
            {
                InjectProperties(toReturn);
            }
            catch (Exception)
            {
                if (!Options.CheckDisposeIfPropertyInjectionFails)
                    throw;

                toReturn.DisposeIfDisposable();
                throw;
            }
            return toReturn;
        }

        public void CallbackWhenRegistered<T>(Action action)
        {
            CallbackWhenRegistered(typeof(T), action);
        }

        public void CallbackWhenRegistered(Type type, Action action)
        {
            lock (mLockObject)
            {
                if (!CanResolve(type))
                {
                    if (mWaiters.TryGetValue(type, out List<Action> actions))
                    {
                        actions.Add(action);
                    }
                    else
                    {
                        actions = new List<Action> { action };
                        mWaiters[type] = actions;
                    }
                    return;
                }
            }

            // if we get here then the type is already registered - so call the aciton immediately
            action();
        }

        public void CleanAllResolvers()
        {
            mResolvers.Clear();
            mWaiters.Clear();
            mCircularTypeDetection.Clear();
        }

        public enum ResolverType
        {
            DynamicPerResolve,
            Singleton,
            Unknown
        }

        public virtual IMvxIoCProvider CreateChildContainer() => new MvxIoCContainer(this);

        private static readonly ResolverType? ResolverTypeNoneSpecified = null;

        private bool Supports(IResolver resolver, ResolverType? requiredResolverType)
        {
            if (!requiredResolverType.HasValue)
                return true;
            return resolver.ResolveType == requiredResolverType.Value;
        }

        private bool InternalTryResolve(Type type, out object resolved)
        {
            return InternalTryResolve(type, ResolverTypeNoneSpecified, out resolved);
        }

        private bool InternalTryResolve(Type type, ResolverType? requiredResolverType, out object resolved)
        {
            if (!TryGetResolver(type, out IResolver resolver))
            {
                if (mParentProvider != null && mParentProvider.TryResolve(type, out resolved))
                {
                    return true;
                }

                resolved = type.CreateDefault();
                return false;
            }

            if (!Supports(resolver, requiredResolverType))
            {
                resolved = type.CreateDefault();
                return false;
            }

            return InternalTryResolve(type, resolver, out resolved);
        }

        private bool TryGetResolver(Type type, out IResolver resolver)
        {
            if (mResolvers.TryGetValue(type, out resolver))
            {
                return true;
            }

            var aa = mResolvers.Keys.ToArray();

            if (!type.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return mResolvers.TryGetValue(type.GetTypeInfo().GetGenericTypeDefinition(), out resolver);
        }

        private bool ShouldDetectCircularReferencesFor(IResolver resolver)
        {
            return resolver.ResolveType switch
            {
                ResolverType.DynamicPerResolve => Options.TryToDetectDynamicCircularReferences,
                ResolverType.Singleton => Options.TryToDetectSingletonCircularReferences,
                ResolverType.Unknown => throw new MvxException("A resolver must have a known type - error in {0}", resolver.GetType().Name),
                _ => throw new ArgumentOutOfRangeException(nameof(resolver), "unknown resolveType of " + resolver.ResolveType),
            };
        }

        private bool InternalTryResolve(Type type, IResolver resolver, out object resolved)
        {
            var detectingCircular = ShouldDetectCircularReferencesFor(resolver);
            if (detectingCircular)
            {
                try
                {
                    mCircularTypeDetection.Add(type, true);
                }
                catch (ArgumentException)
                {
                    // the item already exists in the lookup table
                    // - this is "game over" for the IoC lookup
                    // - see https://github.com/MvvmCross/MvvmCross/issues/553
                    MvxLog.Instance?.Error("IoC circular reference detected - cannot currently resolve {0}", type.Name);
                    resolved = type.CreateDefault();
                    return false;
                }
            }

            try
            {
                if (resolver is ConstructingOpenGenericResolver)
                {
                    resolver.SetGenericTypeParameters(type.GetTypeInfo().GenericTypeArguments);
                }

                var raw = resolver.Resolve();
                if (raw == null)
                {
                    throw new MvxException("Resolver returned null");
                }
                if (!type.IsInstanceOfType(raw))
                {
                    throw new MvxException("Resolver returned object type {0} which does not support interface {1}",
                                           raw.GetType().FullName, type.FullName);
                }

                resolved = raw;
                return true;
            }
            finally
            {
                if (detectingCircular)
                {
                    mCircularTypeDetection.Remove(type);
                }
            }
        }

        private void InternalSetResolver(Type interfaceType, IResolver resolver)
        {
            List<Action> actions;
            lock (mLockObject)
            {
                mResolvers[interfaceType] = resolver;
                if (mWaiters.TryGetValue(interfaceType, out actions))
                {
                    mWaiters.Remove(interfaceType);
                }
            }

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action();
                }
            }
        }

        protected virtual void InjectProperties(object toReturn)
        {
            mPropertyInjector?.Inject(toReturn, mOptions.PropertyInjectorOptions);
        }

        protected virtual List<object> GetIoCParameterValues(Type type, ConstructorInfo selectedConstructor, IDictionary<string, object> arguments)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in selectedConstructor.GetParameters())
            {
                if (arguments?.ContainsKey(parameterInfo.Name) == true)
                {
                    parameters.Add(arguments[parameterInfo.Name]);
                }
                else if (TryResolveParameter(type, parameterInfo, out var parameterValue))
                {
                    parameters.Add(parameterValue);
                }
            }
            return parameters;
        }
        
        protected virtual List<object> GetIoCParameterValues(Type type, ConstructorInfo selectedConstructor, object[] arguments)
        {
            var parameters = new List<object>();
            var unusedArguments = arguments.ToList();
            
            foreach (var parameterInfo in selectedConstructor.GetParameters())
            {
                var argumentMatch = unusedArguments.FirstOrDefault(arg => parameterInfo.ParameterType.IsInstanceOfType(arg));
                
                if (argumentMatch != null)
                {
                    parameters.Add(argumentMatch);
                    unusedArguments.Remove(argumentMatch);
                }
                else if (TryResolveParameter(type, parameterInfo, out var parameterValue))
                {
                    parameters.Add(parameterValue);
                }
            }
            return parameters;
        }

        private bool TryResolveParameter(Type type, ParameterInfo parameterInfo, out object parameterValue)
        {
            if (!TryResolve(parameterInfo.ParameterType, out parameterValue))
            {
                if (parameterInfo.IsOptional)
                {
                    parameterValue = Type.Missing;
                }
                else
                {
                    throw new MvxIoCResolveException(
                        "Failed to resolve parameter for parameter {0} of type {1} when creating {2}. You may pass it as an argument",
                        parameterInfo.Name,
                        parameterInfo.ParameterType.Name,
                        type.FullName);
                }
            }

            return true;
        }
    }
}
