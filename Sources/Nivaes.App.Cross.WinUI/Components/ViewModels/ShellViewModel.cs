using Microsoft.Extensions.Logging;
using Nivaes.App;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class ShellViewModel
        : BaseViewModel
    {
        #region Properties
        //private readonly IAccountConnectorService mAccountConnectorService;

        //public abstract ICrossAsyncCommand ShowSettingsCommand { get; }

        //public abstract ICrossAsyncCommand ShowAccountCommand { get; }

        public readonly IShellModel ShellModel;

        //#region Account
        //private AccountDataModel mAccount;

        //public AccountDataModel Account
        //{
        //    get => mAccount;
        //    set => base.SetProperty(ref mAccount, value);
        //}
        //#endregion

        public bool IsLoaded { get; set; }

        //public abstract IEnumerable<ShellNavigationItem> PrimaryItems { get; }

        //public abstract IEnumerable<ShellNavigationItem> SecondaryItems { get; }
        #endregion

        #region Life cycle
        public ShellViewModel(
            IShellModel shellModel,
            //IAccountConnectorService accountConnectorService, 
            ILogger<ShellViewModel> logger)
           : base(logger)
        {
            //mAccountConnectorService = accountConnectorService;
            ShellModel = shellModel;
        }

        public override async Task Initialize()
        {
            //Account = await mAccountConnectorService.GetAccount();
        }
        #endregion
    }
}
