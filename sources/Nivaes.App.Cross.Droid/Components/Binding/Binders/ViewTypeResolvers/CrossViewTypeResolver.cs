namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class CrossViewTypeResolver
        : IMvxViewTypeResolver
    {
        private readonly Dictionary<string, Type> _cacheFullName = new Dictionary<string, Type>();

        private readonly Dictionary<string, Type> _cacheName = new Dictionary<string, Type>();

        public CrossViewTypeResolver()
        {
            // ToDo: Cargar todas las clases que heredan de Android.Views.View.
            // ToDo: comprobar si es necesario el nuget Xamarin.AndroidX.AppCompat
            var viewList = new List<Type>();
            viewList.Add(typeof(Android.Webkit.WebView));
            viewList.Add(typeof(Android.Widget.LinearLayout));
            viewList.Add(typeof(Android.Widget.ListView));
            viewList.Add(typeof(Android.Views.ViewStub));
            viewList.Add(typeof(Android.Widget.FrameLayout));
            viewList.Add(typeof(Android.Widget.ScrollView));
            viewList.Add(typeof(AndroidX.AppCompat.Widget.FitWindowsLinearLayout));
            viewList.Add(typeof(AndroidX.AppCompat.Widget.ViewStubCompat));
            viewList.Add(typeof(AndroidX.AppCompat.Widget.ContentFrameLayout));

            foreach (var item in viewList)
            {
                if (!string.IsNullOrWhiteSpace(item.FullName))
                    _cacheFullName.Add(item.FullName.ToUpperInvariant(), item);

                _cacheName.Add(item.Name, item);
            }
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? Resolve(string tagName)
        {
            if (_cacheName.TryGetValue(tagName, out Type? toReturn))
                return toReturn;

            if (_cacheFullName.TryGetValue(tagName.ToUpperInvariant(), out toReturn))
                return toReturn;

            //toReturn = _resolver.Resolve(tagName);
            //_cache[tagName] = toReturn;
            //return toReturn;
            return null;
        }
    }
}
