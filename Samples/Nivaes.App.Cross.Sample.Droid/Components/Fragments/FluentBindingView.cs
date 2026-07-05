using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame)]
[RequiresUnreferencedCode("Bindings requires unreferenced code")]
public class FluentBindingView : MvxFragment<FluentBindingViewModel>
{
    EditText? _inputText;
    TextView? _outputText;
    private ICrossInteraction<bool>? _clearBindingInteraction;
    public ICrossInteraction<bool>? ClearBindingInteraction
    {
        get => _clearBindingInteraction;
        set
        {
            _clearBindingInteraction?.Requested -= OnInteractionRequested;
            _clearBindingInteraction = value;
            _clearBindingInteraction?.Requested += OnInteractionRequested;
        }
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.FluentBindingView, null);

        _inputText = view?.FindViewById<EditText>(Resource.Id.inputText);
        _outputText = view?.FindViewById<TextView>(Resource.Id.outputText);
        var toggleButton = view?.FindViewById<Button>(Resource.Id.toggleBtn);

        var bindingSet = CreateBindingSet();
        bindingSet?.Bind(toggleButton!).For(v => v.BindClick()).To(vm => vm.ClearBindingsCommand);
        bindingSet?.Apply();

        BindTextInput();

        return view!;
    }

    [RequiresUnreferencedCode("Bindings require unreferenced code")]
    void BindTextInput()
    {
        var bindingSet = CreateBindingSet();
        bindingSet?.Bind(_inputText!).For(v => v.Text!).To(vm => vm.TextValue);
        bindingSet?.Bind(_outputText!).For(v => v.Text!).To(vm => vm.TextValue);
        bindingSet?.Bind(this).For(v => v.ClearBindingInteraction!).To(vm => vm.ClearBindingInteraction);
        bindingSet?.ApplyWithClearBindingKey(nameof(FluentBindingView));
    }

    private void OnInteractionRequested(object? sender, CrossValueEventArgs<bool> eventArgs)
    {
        if (eventArgs.Value)
            BindTextInput();
        else
            this.ClearBindings(nameof(FluentBindingView));
    }
}
