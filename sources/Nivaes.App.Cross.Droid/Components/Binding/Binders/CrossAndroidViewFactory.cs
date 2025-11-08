namespace Nivaes.App.Cross.Droid
{
    using Android.Content;
    using Android.Util;
    using Android.Views;
    using Microsoft.Extensions.Logging;

    public class CrossAndroidViewFactory
        : ICrossAndroidViewFactory
    {
        private ICrossViewTypeResolver? _viewTypeResolver;

        protected ICrossViewTypeResolver? ViewTypeResolver => throw new NotImplementedException(); //_viewTypeResolver ??= Mvx.IoCProvider?.Resolve<ICrossViewTypeResolver>();

        public virtual View? CreateView(View? parent, string name, Context context, IAttributeSet attrs)
        {
            // resolve the tag name to a type
            var viewType = ViewTypeResolver?.Resolve(name);

            if (viewType == null)
            {
                //CrossBindingLog.Error( "View type not found - {0}", name);
                return null;
            }

            try
            {
                var view = Activator.CreateInstance(viewType, context, attrs) as View;
                if (view == null)
                {
                    CrossBindingLog.Instance?.LogError("Unable to load view {ViewName} from type {ViewTypeName}",
                        name,
                        viewType.FullName);
                }
                return view;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                CrossBindingLog.Instance?.LogError(
                    exception,
                    "Exception during creation of {ViewName} from type {ViewTypeName}", name, viewType.FullName);
                return null;
            }
        }
    }
}
