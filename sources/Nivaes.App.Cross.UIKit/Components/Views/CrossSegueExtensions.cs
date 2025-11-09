namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    internal static class CrossSegueExtensions
    {
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        [UnconditionalSuppressMessage("Trimming", "IL2073", Justification = "PropertyInfo.PropertyType doesn't preserve DynamicallyAccessedMembers annotations, but the ViewModel property type is expected to be a properly constructed ViewModel")]
        [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Runtime type inspection is necessary for segue-based navigation")]
        internal static Type? GetViewModelType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TViewType>(
            this TViewType? view)
                where TViewType : class, ICrossView
        {
            if (view == null)
                return null;

            var props = view.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var prop = Array.Find(props, p => p.Name == "ViewModel");
            return prop?.PropertyType;
        }

        internal static void ViewModelRequestForSegue(this ICrossEventSourceViewController self, UIStoryboardSegue segue, NSObject sender)
        {
            object? parameterValues = null;
            if (self is ICrossIosViewSegue segueView)
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

        private static void ViewModelRequestForSegueImpl(this ICrossEventSourceViewController self, UIStoryboardSegue segue, object? parameterValuesObject)
        {
            self.ViewModelRequestForSegueImpl(segue, parameterValuesObject.ToSimplePropertyDictionary());
        }

        private static void ViewModelRequestForSegueImpl(this ICrossEventSourceViewController self, UIStoryboardSegue segue, IDictionary<string, string> parameterValues)
        {
            self.ViewModelRequestForSegueImpl(segue, new CrossBundle(parameterValues));
        }

        private static void ViewModelRequestForSegueImpl(this ICrossEventSourceViewController _, UIStoryboardSegue segue, ICrossBundle? parameterBundle = null)
        {
            throw new NotImplementedException();
            //if (segue.DestinationViewController is ICrossIosView view && view.Request == null)
            //{
            //    var type = view.GetViewModelType();
            //    if (type != null)
            //    {
            //        view.Request = new CrossViewModelRequest(type, parameterBundle, null);
            //    }
            //}
        }
    }
}
