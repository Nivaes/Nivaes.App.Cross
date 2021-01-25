namespace Nivaes.App.Cross.Mobele.Windows.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Nivaes.App.Cross.Mobile.Sample;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.Windows;

    public sealed class Setup
        : MvxWindowsSetup<MobileSampleCrossApplication<AppMobileSampleAppStart>>
    {
        [Obsolete("Not user reflector")]
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
