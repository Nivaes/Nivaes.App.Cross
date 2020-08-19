namespace Nivaes.App.Cross.Mobele.Windows.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Program
    {
        public static void Main(string[] args)
        {
            WinRT.ComWrappersSupport.InitializeComWrappers();

            Microsoft.UI.Xaml.Application.Start((e) => new Mobile.Windows.Sample.App());
        }
    }
}
