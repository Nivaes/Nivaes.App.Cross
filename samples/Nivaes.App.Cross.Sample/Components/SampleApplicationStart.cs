namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;

    public class SampleApplicationStart 
        : CrossApplicationStart
    {
        private readonly ICrossNavigationService mNavigationService;

        public SampleApplicationStart(ICrossNavigationService navigationService)
        {
            mNavigationService = navigationService;

            // ToDo: Cargar esto con roslyn
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.Merge(new ViewModelsSubcontainer());
        }

        public override async Task NavigateToFirstViewModel(object? hint = null)
        {
            try
            {
                await mNavigationService.Navigate<RootViewModel>();
            }
            catch (System.Exception exception)
            {
                throw exception.Wrap("Problem navigating to ViewModel {0}", typeof(RootViewModel).Name);
            }
        }
    }
}
