using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace Nivaes.App.Cross.WinUI;

public class AutoSuggestBoxBehavior
    : Behavior<AutoSuggestBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        base.AssociatedObject.TextChanged += OnTextChanged;
        base.AssociatedObject.QuerySubmitted += OnQuerySubmitted;
    }

    protected override void OnDetaching()
    {
        base.AssociatedObject.TextChanged -= OnTextChanged;
        base.AssociatedObject.QuerySubmitted -= OnQuerySubmitted;

        base.OnDetaching();
    }

    #region SuggestionsCommand
    public ICommand SuggestionsCommand
    {
        get => (ICommand)base.GetValue(SuggestionsCommandProperty);
        set => base.SetValue(SuggestionsCommandProperty, value);
    }

    public static readonly DependencyProperty SuggestionsCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(SuggestionsCommand),
            typeof(ICommand),
            typeof(AutoSuggestBoxBehavior),
            new PropertyMetadata(null));
    #endregion

    #region SearchCommand
    public ICommand SearchCommand
    {
        get => (ICommand)base.GetValue(SearchCommandProperty);
        set => base.SetValue(SearchCommandProperty, value);
    }

    public static readonly DependencyProperty SearchCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(SearchCommand),
            typeof(ICommand),
            typeof(AutoSuggestBoxBehavior),
            new PropertyMetadata(null));
    #endregion

    private void OnTextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (SuggestionsCommand?.CanExecute(base.AssociatedObject.Text) ?? false)
            {
                SuggestionsCommand.Execute(base.AssociatedObject.Text);
            }

            if (SearchCommand?.CanExecute(base.AssociatedObject.Text) ?? false)
            {
                SearchCommand.Execute(base.AssociatedObject.Text);
            }
        }
    }

    private void OnQuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        if (SearchCommand?.CanExecute(e.QueryText) ?? false)
        {
            SearchCommand.Execute(e.QueryText);
        }
    }
}
