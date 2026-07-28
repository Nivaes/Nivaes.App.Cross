using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Xaml.Interactivity;

namespace Nivaes.App.Cross.WinUI
{
    public class ListViewBaseBehavior
        : Behavior<ListViewBase>
    {
        public static string AnimatedKey = "_ForwardConnectedAnimation";

        protected override void OnAttached()
        {
            base.OnAttached();

            base.AssociatedObject.IsItemClickEnabled = true;
            base.AssociatedObject.ItemClick += OnItemClick;
        }

        protected override void OnDetaching()
        {
            base.AssociatedObject.ItemClick -= OnItemClick;

            base.OnDetaching();
        }

        #region ItemClick

        #region AnimatedElementName
        public static readonly DependencyProperty ConnectedAnimatedElementNameProperty =
            DependencyProperty.Register(
            nameof(ConnectedAnimatedElementName),
            typeof(string),
            typeof(ListViewBaseBehavior),
            new PropertyMetadata(null));

        public string ConnectedAnimatedElementName
        {
            get => (string)GetValue(ConnectedAnimatedElementNameProperty);
            set => SetValue(ConnectedAnimatedElementNameProperty, value);
        }
        #endregion

        #region ItemClick
        public ICommand ItemClickCommand
        {
            get => (ICommand)base.GetValue(ItemClickCommandProperty);
            set => base.SetValue(ItemClickCommandProperty, value);
        }

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.RegisterAttached(
                nameof(ItemClickCommand),
                typeof(ICommand),
                typeof(ListViewBaseBehavior),
                new PropertyMetadata(null));
        #endregion

        private DateTime _lastItemClickAction = DateTime.UtcNow;

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (_lastItemClickAction < DateTime.UtcNow)
            {
                var command = ItemClickCommand;

                if (command != null)
                {
                    var container = (SelectorItem)base.AssociatedObject.ContainerFromItem(e.ClickedItem);
                    var selectedItem = e.ClickedItem;

                    if (!string.IsNullOrEmpty(ConnectedAnimatedElementName))
                    {
                        var image = container.FindControl<FrameworkElement>(ConnectedAnimatedElementName);

                        ConnectedAnimationService
                            .GetForCurrentView()
                            .PrepareToAnimate(AnimatedKey, image);
                    }

                    if (command.CanExecute(selectedItem))
                        command.Execute(selectedItem);

                    _lastItemClickAction = DateTime.UtcNow.AddSeconds(0.5);
                }
            }
        }
        #endregion

        public static void AnimationStart(UIElement destination, IEnumerable<UIElement> coordinatedElements)
        {
            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation(AnimatedKey);
            if (imageAnimation != null)
            {
                imageAnimation.TryStart(destination, coordinatedElements);

            }
        }
    }
}
