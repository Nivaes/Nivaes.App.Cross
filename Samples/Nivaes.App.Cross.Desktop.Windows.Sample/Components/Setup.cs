namespace Nivaes.App.Cross.Desktop.Windows.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.Wpf;
    using Nivaes.App.Desktop.Sample;

    public sealed class Setup
        : MvxWpfSetup<AppDesktopSampleApplication<AppDesktopSampleAppStart>>
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
