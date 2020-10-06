﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Base;
using Nivaes.App.Cross.Logging;
using MvvmCross.Tests;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    public class NavigationMockDispatcher
        : IMvxMainThreadDispatcher, IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

        public bool IsOnMainThread => true;

        public virtual bool RequestMainThreadAction(Action action,
                                                    bool maskExceptions = true)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception)
            {
                if (!maskExceptions)
                    throw;

                return false;
            }
        }

        public virtual ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            var debugString = $"ShowViewModel: '{request.ViewModelType.Name}' ";
            if (request.ParameterValues != null)
                debugString += $"with parameters: {string.Join(",", request.ParameterValues.Select(pair => $"{{ {pair.Key}={pair.Value} }}"))}";
            else
                debugString += "without parameters";
            MvxTestLog.Instance.Log(MvxLogLevel.Debug, () => debugString);

            Requests.Add(request);
            return new ValueTask<bool>(true);
        }

        public virtual ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            Hints.Add(hint);
            return new ValueTask<bool>(true);
        }

        public ValueTask ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            return new ValueTask(Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception) when (maskExceptions)
                {
                }
            }));
        }

        public async ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            try
            {
                await action();
            }
            catch (Exception) when (maskExceptions)
            {
            }
        }
    }
}
