namespace Nivaes.App
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>The IdentiyService <c>interface</c>.</summary>
    public interface IIdentifyService
    {
        bool EdidHonorificPrefixes { get; }

        Task InitializingApp();

        /// <summary>The login method called with account supplied credentials.</summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The users password.</param>
        /// <returns><c>true</c> if the login is successful, <c>false</c> otherwise </returns>
        ValueTask<bool> AccountIdentify(string userName, string password);

        //ValueTask<(bool accountCreated, Guid idAccount, CommunicationError errors)> CreateAccount(NewAccountModel newAccount);

        //Task CreateUser(UserDataModel user);

        /// <summary>Initialize app.</summary>
        Task InitializeApp();

        /// <summary>Load user data.</summary>
        Task InitializeUser();

        Task LoadData();

        //ValueTask<(bool IsValid, IEnumerable<IdentityErrorResponse> Errors)> TryValidatePassword(string password, CancellationToken cancellationToken);
    }
}
