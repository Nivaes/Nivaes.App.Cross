// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC;

[Obsolete("Quitar MvxIoC", true)]
public interface IMvxIoCProvider
{
    [Obsolete("Quitar MvxIoC", true)]
    bool CanResolve<T>()
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    bool CanResolve(Type type);

    [Obsolete("Quitar MvxIoC", true)]
    T? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    object? Resolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

    [Obsolete("Quitar MvxIoC", true)]
    bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(out T? resolved)
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    bool TryResolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, out object? resolved);

    [Obsolete("Quitar MvxIoC", true)]
    T? Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    object? Create([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

    [Obsolete("Quitar MvxIoC", true)]
    T? GetSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    object? GetSingleton([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterType<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTo>()
        where TFrom : class
        where TTo : class, TFrom;

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterType<TInterface>(Func<TInterface> constructor)
        where TInterface : class;

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterType(Type t, Func<object> constructor);

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterType(Type tFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type tTo);

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterSingleton<TInterface>(TInterface theObject)
        where TInterface : class;

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterSingleton(Type tInterface, object theObject);

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
        where TInterface : class;

    [Obsolete("Quitar MvxIoC", true)]
    void RegisterSingleton(Type tInterface, Func<object> theConstructor);

    [Obsolete("Quitar MvxIoC", true)]
    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IDictionary<string, object>? arguments)
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(object? arguments)
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(params object?[] arguments)
        where T : class;

    [Obsolete("Quitar MvxIoC", true)]
    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type);

    [Obsolete("Quitar MvxIoC", true)]
    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, IDictionary<string, object>? arguments);

    [Obsolete("Quitar MvxIoC", true)]
    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, object? arguments);

    [Obsolete("Quitar MvxIoC", true)]
    object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, params object?[] arguments);

    [Obsolete("Quitar MvxIoC", true)]
    IMvxIoCProvider CreateChildContainer();
}
