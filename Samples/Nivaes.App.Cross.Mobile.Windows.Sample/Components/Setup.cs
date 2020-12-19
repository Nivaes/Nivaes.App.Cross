namespace Nivaes.App.Cross.Mobele.Windows.Sample
{
    using System.Collections.Generic;
    using System.Reflection;
    using MvvmCross.Platforms.Uap.Core;
    using Nivaes.App.Cross.Mobile.Sample;
    using Nivaes.App.Cross.Sample;

    public sealed class Setup
        : MvxWindowsSetup<AppMobileSampleApplication<AppMobileSampleAppStart>>
    {
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new List<Assembly>(base.GetViewModelAssemblies())
            {
                typeof(RootViewModel).GetTypeInfo().Assembly
            };
        }

        //public override IEnumerable<Assembly> GetViewAssemblies()
        //{
        //    return new List<Assembly>(base.GetViewAssemblies())
        //    {
        //        typeof(RootView).GetTypeInfo().Assembly
        //    };
        //}
    }
}
