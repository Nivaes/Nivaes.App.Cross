namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    [Obsolete]
    public class CrossViewModelResult<TResult>
        : CrossViewModel, ICrossViewModelResult<TResult>
    {
        public TaskCompletionSource<object>? CloseCompletionSource 
        { 
            get; 
            set; 
        }
    }
}
