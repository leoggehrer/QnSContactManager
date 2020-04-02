//@QnSCodeCopy
//MdStart
using System.Threading.Tasks;
using QnSContactManager.Contracts.Persistence.Account;

namespace QnSContactManager.Adapters.Modules.Account
{
    public partial class AccountManager
    {
        static AccountManager()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        /// <summary>
        /// The base url like https://localhost:5001/api
        /// </summary>
        public string BaseUri { get; set; } = Factory.BaseUri;
        public AdapterType Adapter { get; set; } = Factory.Adapter;

        public AccountManager()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public async Task<ILoginSession> LogonAsync(string jsonWebToken)
        {
            var result = default(ILoginSession);

            if (Adapter == AdapterType.Controller)
            {
                result = await Logic.Modules.Account.AccountManager.LogonAsync(jsonWebToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.LogonAsync(jsonWebToken).ConfigureAwait(false);
            }
            return result;
        }
        public async Task<ILoginSession> LogonAsync(string email, string password)
        {
            var result = default(ILoginSession);

            if (Adapter == AdapterType.Controller)
            {
                result = await Logic.Modules.Account.AccountManager.LogonAsync(email, password).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.LogonAsync(email, password).ConfigureAwait(false);
            }
            return result;
        }
        public async Task LogoutAsync(string sessionToken)
        {
            if (Adapter == AdapterType.Controller)
            {
                await Logic.Modules.Account.AccountManager.LogoutAsync(sessionToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                serviceInvoker.SessionToken = sessionToken;
                await serviceInvoker.LogoutAsync(sessionToken).ConfigureAwait(false);
            }
        }
        public async Task ChangePasswordAsync(string sessionToken, string oldPwd, string newPwd)
        {
            if (Adapter == AdapterType.Controller)
            {
                await Logic.Modules.Account.AccountManager.ChangePasswordAsync(sessionToken, oldPwd, newPwd).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                await serviceInvoker.ChangePasswordAsync(sessionToken, oldPwd, newPwd).ConfigureAwait(false);
            }
        }
        public async Task ChangePasswordForAsync(string sessionToken, string email, string password)
        {
            if (Adapter == AdapterType.Controller)
            {
                await Logic.Modules.Account.AccountManager.ChangePasswordForAsync(sessionToken, email, password).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                await serviceInvoker.ChangePasswordForAsync(sessionToken, email, password).ConfigureAwait(false);
            }
        }
        public async Task ResetForAsync(string sessionToken, string email)
        {
            if (Adapter == AdapterType.Controller)
            {
                await Logic.Modules.Account.AccountManager.ResetForAsync(sessionToken, email).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                await serviceInvoker.ResetForAsync(sessionToken, email).ConfigureAwait(false);
            }
        }
        public async Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            var result = false;

            if (Adapter == AdapterType.Controller)
            {
                result = await Logic.Modules.Account.AccountManager.HasRoleAsync(sessionToken, role).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.HasRoleAsync(sessionToken, role).ConfigureAwait(false);
            }
            return result;
        }
        public async Task<ILoginSession> QueryLoginAsync(string sessionToken)
        {
            var result = default(ILoginSession);

            if (Adapter == AdapterType.Controller)
            {
                result = await Logic.Modules.Account.AccountManager.QueryLoginAsync(sessionToken).ConfigureAwait(false);
            }
            else if (Adapter == AdapterType.Service)
            {
                var serviceInvoker = new Service.InvokeServiceAdapter(BaseUri);

                result = await serviceInvoker.QueryLoginAsync(sessionToken).ConfigureAwait(false);
            }
            return result;
        }
    }
}
//MdEnd
