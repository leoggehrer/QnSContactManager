//@QnSCodeCopy
//MdStart
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QnSContactManager.Contracts.Persistence.Account;

namespace QnSContactManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AccountController : ControllerBase
    {
        private ILoginSession ConvertTo(ILoginSession loginSession)
        {
            var result = new Transfer.Persistence.Account.LoginSession();

            result.CopyProperties(loginSession);
            return result;
        }
        [HttpGet("/api/[controller]/Logon/{jwt}")]
        public async Task<ILoginSession> LogonAsync(string jwt)
        {
            return ConvertTo(await Logic.Modules.Account.AccountManager.LogonAsync(jwt).ConfigureAwait(false));
        }
        [HttpGet("/api/[controller]/Logon/{email}/{password}")]
        public async Task<ILoginSession> LogonAsync(string email, string password)
        {
            return ConvertTo(await Logic.Modules.Account.AccountManager.LogonAsync(email, password).ConfigureAwait(false));
        }
        [HttpGet("/api/[controller]/Logout/{sessionToken}")]
        public Task LogoutAsync(string sessionToken)
        {
            return Logic.Modules.Account.AccountManager.LogoutAsync(sessionToken);
        }
        [HttpGet("/api/[controller]/ChangePassword/{sessionToken}/{oldPwd}/{newPwd}")]
        public Task ChangePasswordAsync(string sessionToken, string oldPwd, string newPwd)
        {
            return Logic.Modules.Account.AccountManager.ChangePasswordAsync(sessionToken, oldPwd, newPwd);
        }
        [HttpGet("/api/[controller]/ChangePasswordFor/{sessionToken}/{email}/{newPwd}")]
        public Task ChangePasswordForAsync(string sessionToken, string email, string newPwd)
        {
            return Logic.Modules.Account.AccountManager.ChangePasswordForAsync(sessionToken, email, newPwd);
        }
        [HttpGet("/api/[controller]/ResetFor/{sessionToken}/{email}")]
        public Task ChangeForAsync(string sessionToken, string email)
        {
            return Logic.Modules.Account.AccountManager.ResetForAsync(sessionToken, email);
        }
        [HttpGet("/api/[controller]/HasRole/{sessionToken}/{role}")]
        public Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            return Logic.Modules.Account.AccountManager.HasRoleAsync(sessionToken, role);
        }

        [HttpGet("/api/[controller]/QueryLogin/{sessionToken}")]
        public async Task<ILoginSession> QueryLoginAsync(string sessionToken)
        {
            return ConvertTo(await Logic.Modules.Account.AccountManager.QueryLoginAsync(sessionToken).ConfigureAwait(false));
        }
    }
}
//MdEnd
