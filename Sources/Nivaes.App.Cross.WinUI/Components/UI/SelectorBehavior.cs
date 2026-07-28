using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

namespace Nivaes.App.Cross.WinUI
{
    public class SelectorBehavior
        : Behavior<Selector>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            base.AssociatedObject.Tapped += OnTaped;
            base.AssociatedObject.SelectionChanged += OnSelectedItenChanged;
        }

        protected override void OnDetaching()
        {
            base.AssociatedObject.SelectionChanged -= OnSelectedItenChanged;
            base.AssociatedObject.Tapped -= OnTaped;

            base.OnDetaching();
        }

        #region SelectedItemCommand
        public ICommand SelectedItemCommand
        {
            get => (ICommand)base.GetValue(SelectedItemCommandProperty);
            set => base.SetValue(SelectedItemCommandProperty, value);
        }

        public static readonly DependencyProperty SelectedItemCommandProperty =
            DependencyProperty.RegisterAttached(
                nameof(SelectedItemCommand),
                typeof(ICommand),
                typeof(SelectorBehavior),
                new PropertyMetadata(null));

        private DateTime _lastSelectedItenAction = DateTime.UtcNow;

        private void OnSelectedItenChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_lastSelectedItenAction < DateTime.UtcNow)
            {
                var command = SelectedItemCommand;

                if (command != null)
                {
                    //var container = (SelectorItem)base.AssociatedObject.ContainerFromItem(e.AddedItems);
                    //var image = container.FindControl<Image>();
                    //ConnectedAnimationService
                    //    .GetForCurrentView()
                    //    .PrepareToAnimate("ForwardConnectedAnimation", image);

                    var selectedItem = base.AssociatedObject.SelectedItem;

                    if (command.CanExecute(selectedItem))
                        command.Execute(selectedItem);

                    _lastSelectedItenAction = DateTime.UtcNow.AddSeconds(0.5);
                }
            }
        }
        #endregion

        #region TapedCommand
        public ICommand TapedCommand
        {
            get => (ICommand)base.GetValue(TapedCommandProperty);
            set => base.SetValue(TapedCommandProperty, value);
        }

        public static readonly DependencyProperty TapedCommandProperty =
            DependencyProperty.RegisterAttached(
                nameof(TapedCommand),
                typeof(ICommand),
                typeof(Selector),
                new PropertyMetadata(null));

        private DateTime _lastTapedAction = DateTime.UtcNow;
        private static object? _lastSelectedObject;

        private void OnTaped(object sender, TappedRoutedEventArgs e)
        {
            if (_lastTapedAction < DateTime.UtcNow)
            {
                var command = TapedCommand;

                if (command != null)
                {
                    var selectedItem = base.AssociatedObject.SelectedItem;

                    if (_lastSelectedObject != selectedItem)
                    {
                        _lastSelectedObject = selectedItem;

                        if (command.CanExecute(selectedItem))
                            command.Execute(selectedItem);
                    }

                    _lastTapedAction = DateTime.UtcNow.AddSeconds(0.5);
                }
            }
        }
        #endregion
    }
}
