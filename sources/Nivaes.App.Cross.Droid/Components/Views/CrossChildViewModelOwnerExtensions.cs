// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Android.Content;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace Nivaes.App.Cross.Droid
{
    public static class CrossChildViewModelOwnerExtensions
    {
        public static Intent CreateIntentFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(this ICrossAndroidView view, object parameterObject)
            where TTargetViewModel : class, ICrossViewModel
        {
            return view.CreateIntentFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());
        }

        public static Intent CreateIntentFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTargetViewModel>(
        this ICrossAndroidView view,
        IDictionary<string, string> parameterValues = null)
        where TTargetViewModel : class, ICrossViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new CrossViewModelRequest<TTargetViewModel>(parameterBundle, null);
            return view.CreateIntentFor(request);
        }

        public static Intent CreateIntentFor(this ICrossAndroidView view, CrossViewModelRequest request)
        {
            return Mvx.IoCProvider.Resolve<ICrossAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        public static Intent CreateIntentFor(this ICrossChildViewModelOwner view, ICrossViewModel subViewModel)
        {
            var requestTranslator = Mvx.IoCProvider.Resolve<ICrossAndroidViewModelRequestTranslator>();
            var (intent, key) = requestTranslator.GetIntentWithKeyFor(subViewModel, null);

            view.OwnedSubViewModelIndicies.Add(key);

            return intent;
        }

        public static void ClearOwnedSubIndicies(this ICrossChildViewModelOwner view)
        {
            var translator = Mvx.IoCProvider.Resolve<ICrossAndroidViewModelRequestTranslator>();
            foreach (var ownedSubViewModelIndex in view.OwnedSubViewModelIndicies)
            {
                translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            }
            view.OwnedSubViewModelIndicies.Clear();
        }
    }
}
