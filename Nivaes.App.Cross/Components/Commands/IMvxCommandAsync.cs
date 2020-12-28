// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System.Threading.Tasks;

    public interface IMvxCommandAsync
        : IMvxCommand
    {
        ValueTask ExecuteAsync(object? parameter = null);

        void Cancel();
    }

    public interface IMvxCommandAsync<T>
        : IMvxCommand<T>
    {
        ValueTask ExecuteAsync(T parameter);

        void Cancel();
    }
}
