using Android.Views;
using Google.Android.Material.Navigation;

namespace Nivaes.App.Cross.Droid
{
    public abstract class BaseMenuView<TMenuViewModel>
        : MvxFragment<TMenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
        where TMenuViewModel : BaseMenuViewModel
    {
        protected NavigationView? NavigationView { get; private set; }

        private static int mSelectedItemId = 0;

        protected abstract int MenuResourceId { get; }
        protected abstract int DefaultItemId { get; }

        protected abstract MenuItemCollection MenuItems { get; }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.menu_view, null);

            NavigationView = view?.FindViewById<NavigationView>(Resource.Id.navigation_view);
            NavigationView?.InflateMenu(MenuResourceId);
            NavigationView?.SetNavigationItemSelectedListener(this);

            if (mSelectedItemId == 0)
            {
                mSelectedItemId = DefaultItemId;
            }
            IMenuItem? selectedMenuItem = NavigationView?.Menu.FindItem(mSelectedItemId);
            selectedMenuItem?.SetChecked(true);

            MenuLabels(NavigationView?.Menu);

            return view;
        }

        bool NavigationView.IOnNavigationItemSelectedListener.OnNavigationItemSelected(IMenuItem item)
        {
            if (item.IsCheckable)
            {
                IMenuItem? selectedMenuItem = NavigationView?.Menu.FindItem(mSelectedItemId);

                if (item != selectedMenuItem)
                {
                    selectedMenuItem?.SetChecked(false);
                }

                item?.SetChecked(true);
                mSelectedItemId = item.ItemId;
            }

            NavigateInternal(item.ItemId);

            return true;
        }

        private async void NavigateInternal(int itemId)
        {
#if true
            ((IMainActivity)Activity).DrawerLayout?.CloseDrawers();
            await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
#else
            ((IMainActivity)Activity).DrawerLayout?.CloseDrawer((int)GravityFlags.Left, false);
#endif

            await Navigate(itemId).ConfigureAwait(false);
        }

        private void MenuLabels(IMenu? menu)
        {
            foreach (var menuItem in MenuItems)
            {
                var menuIt = menu?.FindItem(menuItem.ItemId);
                if (menuIt != null)
                {
                    menuIt.SetTitle(menuItem.Label);
                }
            }
        }

        private async Task Navigate(int itemId)
        {
            if (MenuItems.Contains(itemId))
            {
                var menuItem = MenuItems[itemId];

                await (menuItem.Command?.ExecuteAsync()).ConfigureAwait(false);
            }
        }
    }
}
