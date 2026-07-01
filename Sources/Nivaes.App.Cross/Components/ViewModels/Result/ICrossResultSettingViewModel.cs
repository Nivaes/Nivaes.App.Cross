namespace Nivaes.App.Cross
{
    public interface ICrossResultSettingViewModel<in TResult>
    {
        void SetResult(TResult result);
    }
}