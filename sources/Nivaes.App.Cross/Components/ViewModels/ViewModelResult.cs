namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public class ViewModelResult<TResult>
        : ViewModel, IViewModelResult<TResult>
    {
        public TaskCompletionSource<object> CloseCompletionSource 
        { 
            get; 
            set; 
        }
    }
}
