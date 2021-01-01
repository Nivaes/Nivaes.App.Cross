namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using MvvmCross;
    using MvvmCross.IoC;
    using Nivaes.App.Cross.ViewModels;

    public class AppSampleApplication<TType>
        : MvxApplication
          where TType : class, IMvxAppStart
    {
        public override ValueTask Initialize()
        {
            // Construct custom application start object
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, TType>();
            // request a reference to the constructed appstart object 
            var appStart = Mvx.IoCProvider.Resolve<IMvxAppStart>();
            // register the appstart object
            base.RegisterAppStart(appStart);

            return base.Initialize();
        }
    }
}
