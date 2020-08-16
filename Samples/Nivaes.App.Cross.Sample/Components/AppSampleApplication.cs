namespace Nivaes.App.Cross.Sample
{
    using MvvmCross;
    using MvvmCross.IoC;
    using MvvmCross.ViewModels;

    //ToDo: Crear clase base en Nivaes.App
    public class AppSampleApplication<TType>
        : MvxApplication
          where TType : class, IMvxAppStart
    {
        public override void Initialize()
        {
            // Construct custom application start object
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, TType>();
            // request a reference to the constructed appstart object 
            var appStart = Mvx.IoCProvider.Resolve<IMvxAppStart>();
            // register the appstart object
            base.RegisterAppStart(appStart);

            base.Initialize();
        }
    }
}
