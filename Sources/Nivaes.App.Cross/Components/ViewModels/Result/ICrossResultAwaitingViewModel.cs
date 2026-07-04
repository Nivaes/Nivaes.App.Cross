namespace Nivaes.App.Cross
{
    /// <summary>
    /// Implementing this interface requires to manually register and unregister ViewModel to result:
    /// <code>
    /// protected override void ReloadFromBundle(IMvxBundle state)
    /// {
    ///     base.ReloadFromBundle(state);
    ///     this.ReloadAndRegisterToResult&lt;<typeparamref name="TResult"/>&gt;(state, IPlatformApplication.Current!.Services.GetRequiredService&lt;IMvxResultViewModelManager&gt;());
    /// }
    /// protected override void SaveStateToBundle(IMvxBundle bundle)
    /// {
    ///     base.SaveStateToBundle(bundle);
    ///     this.SaveRegisterToResult&lt;<typeparamref name="TResult"/>&gt;(bundle, IPlatformApplication.Current!.Services.GetRequiredService&lt;IMvxResultViewModelManager&gt;());
    /// }
    /// public override ViewDestroy(bool viewFinishing = true)
    /// {
    ///     base.ViewDestroy();
    ///     if (viewFinishing)
    ///         this.UnregisterToResult&lt;<typeparamref name="TResult"/>&gt;(IPlatformApplication.Current!.Services.GetRequiredService&lt;IMvxResultViewModelManager&gt;());
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface ICrossResultAwaitingViewModel<TResult> : IMvxBaseResultAwaitingViewModel
    {
        bool ResultSet(ICrossResultSettingViewModel<TResult> viewModel, TResult result);
    }

    public interface IMvxBaseResultAwaitingViewModel
    {
    }
}