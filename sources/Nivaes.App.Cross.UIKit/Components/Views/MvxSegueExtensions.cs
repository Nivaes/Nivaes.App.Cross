namespace Nivaes.App.Cross.UIKitOS
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Foundation;
    using UIKit;

    internal static class MvxSegueExtensions
    {
        extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TViewType>(TViewType? view)
            where TViewType : class, ICrossView
        {
            [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
            internal Type? GetViewModelType()
            {
                if (view == null)
                    return null;

                var props = view.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var prop = Array.Find(props, p => p.Name == "ViewModel");
                return prop?.PropertyType;
            }
        }

        extension(IMvxEventSourceViewController self)
        {
            internal void ViewModelRequestForSegue(UIStoryboardSegue segue, NSObject? sender)
            {
                object? parameterValues = null;
                if (self is IMvxIosViewSegue segueView)
                {
                    parameterValues = segueView.PrepareViewModelParametersForSegue(segue, sender);
                }

                if (parameterValues is ICrossBundle bundleValues)
                    self.ViewModelRequestForSegueImpl(segue, bundleValues);
                else if (parameterValues is IDictionary<string, string> dictValues)
                    self.ViewModelRequestForSegueImpl(segue, dictValues);
                else
                    self.ViewModelRequestForSegueImpl(segue, parameterValues);
            }

            private void ViewModelRequestForSegueImpl(UIStoryboardSegue segue, object? parameterValuesObject)
            {
                self.ViewModelRequestForSegueImpl(segue, parameterValuesObject.ToSimplePropertyDictionary());
            }

            private void ViewModelRequestForSegueImpl(UIStoryboardSegue segue, IDictionary<string, string> parameterValues)
            {
                self.ViewModelRequestForSegueImpl(segue, new CrossBundle(parameterValues));
            }

            private static void ViewModelRequestForSegueImpl(UIStoryboardSegue segue, ICrossBundle? parameterBundle = null)
            {
                if (segue.DestinationViewController is IMvxIosView view && view.Request == null)
                {
                    var type = view.GetViewModelType();
                    if (type != null)
                    {
                        view.Request = new CrossViewModelRequest(type, parameterBundle, null);
                    }
                }
            }
        }
    }
}
