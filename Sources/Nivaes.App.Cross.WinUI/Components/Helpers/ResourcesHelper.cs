using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Nivaes.App.Cross.WinUI
{
    public static class ResourcesHelper
    {
        public static string GetString(string resourceName)
        {
            return (string)Application.Current.Resources[resourceName];
        }

        public static IconElement? GetIcon(string resourceName)
        {
            var icon = (PathIcon)Application.Current.Resources[resourceName];

            PathGeometry geometry = (PathGeometry)icon.Data;

            try
            {
                return new PathIcon
                {
                    Data = new PathGeometry
                    {
                        Figures = geometry.Figures
                    }
                };
            }
            catch
            {
                return null;
            }
        }

        public static Canvas GetCanvas(string resourceName)
        {
            return (Canvas)Application.Current.Resources[resourceName];
        }

        public static DataTemplate GetDataTemplate(string resourceName)
        {
            return (DataTemplate)Application.Current.Resources[resourceName];
        }

        public static DependencyObject GetDependencyObject(string resourceName)
        {
            return (DependencyObject)Application.Current.Resources[resourceName];
        }
    }
}
