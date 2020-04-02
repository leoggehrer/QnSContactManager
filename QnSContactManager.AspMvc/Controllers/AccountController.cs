//@QnSCodeCopy
//MdStart
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CommonBase.Extensions;
using CommonBase.Helpers;
using QnSContactManager.AspMvc.Models.Modules.Account;
using QnSContactManager.AspMvc.Models.Persistence.Account;
using Model = QnSContactManager.AspMvc.Models.Persistence.Account.LoginSession;
using Contract = QnSContactManager.Contracts.Persistence.Account.ILoginSession;
using AccountManager = QnSContactManager.Adapters.Modules.Account.AccountManager;

namespace QnSContactManager.AspMvc.Controllers
{
    public partial class AccountController : MvcController
    {
        static AccountController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, IFactoryWrapper factoryWrapper)
            : base(factoryWrapper)
        {
            Constructing();
            _logger = logger;
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public IActionResult Logon(string returnUrl = null, string error = null)
        {
            var handled = false;
            var viewName = nameof(Logon);
            var viewModel = new LogonViewModel()
            {
                ReturnUrl = returnUrl,
                ActionError = error,
            };

            BeforeLogon(viewModel, ref handled);
            if (handled == false)
            {
                SessionWrapper.ReturnUrl = viewModel.ReturnUrl;
                SessionWrapper.Error = viewModel.ActionError;
            }
            AfterLogon(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        partial void BeforeLogon(LogonViewModel model, ref bool handled);
        partial void AfterLogon(LogonViewModel model, ref string viewName);

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Logon")]
        public async Task<IActionResult> LogonAsync(LogonViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(viewModel);
            }
            bool handled = false;
            var action = "Index";
            var controller = "Home";

            BeforeDoLogon(viewModel, ref handled);
            if (handled == false)
            {
                try
                {
                    if (string.IsNullOrEmpty(viewModel.IdentityUrl))
                    {
                        await ExecuteLogonAsync(viewModel).ConfigureAwait(false);
                    }
                    else
                    {
                        await ExecuteLogonRemoteAsync(viewModel).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    viewModel.ActionError = ex.Message;
                    return View(viewModel);
                }
            }
            AfterDoLogon(viewModel, ref action, ref controller);
            if (viewModel.ReturnUrl.HasContent())
            {
                return Redirect(viewModel.ReturnUrl);
            }
            return RedirectToAction(action, controller);
        }
        partial void BeforeDoLogon(LogonViewModel model, ref bool handled);
        partial void AfterDoLogon(LogonViewModel model, ref string action, ref string controller);

        public IActionResult LogonRemote(string returnUrl = null, string error = null)
        {
            var handled = false;
            var viewName = nameof(LogonRemote);
            var viewModel = new LogonViewModel()
            {
                ReturnUrl = returnUrl,
                ActionError = error,
            };

            BeforeLogonRemote(viewModel, ref handled);
            if (handled == false)
            {
                SessionWrapper.ReturnUrl = viewModel.ReturnUrl;
                SessionWrapper.Error = viewModel.ActionError;
            }
            AfterLogonRemote(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        partial void BeforeLogonRemote(LogonViewModel model, ref bool handled);
        partial void AfterLogonRemote(LogonViewModel model, ref string viewName);

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("LogonRemote")]
        public async Task<IActionResult> LogonRemoteAsync(LogonViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(viewModel);
            }
            bool handled = false;
            var action = "Index";
            var controller = "Home";

            BeforeDoLogonRemote(viewModel, ref handled);
            if (handled == false)
            {
                try
                {
                    await ExecuteLogonRemoteAsync(viewModel).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    viewModel.ActionError = ex.Message;
                    return View(viewModel);
                }
            }
            AfterDoLogonRemote(viewModel, ref action, ref controller);
            if (viewModel.ReturnUrl.HasContent())
            {
                return Redirect(viewModel.ReturnUrl);
            }
            return RedirectToAction(action, controller);
        }
        partial void BeforeDoLogonRemote(LogonViewModel model, ref bool handled);
        partial void AfterDoLogonRemote(LogonViewModel model, ref string action, ref string controller);

        [ActionName("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            if (SessionWrapper.LoginSession != null)
            {
                bool handled = false;

                BeforeLogout(ref handled);
                if (handled == false)
                {
                    var accMngr = new AccountManager();

                    await accMngr.LogoutAsync(SessionWrapper.LoginSession.SessionToken).ConfigureAwait(false);
                    SessionWrapper.LoginSession = null;
                }
                AfterLogout();
            }
            return RedirectToAction("Index", "Home");
        }
        partial void BeforeLogout(ref bool handled);
        partial void AfterLogout();

        public IActionResult ChangePassword()
        {
            var handled = false;
            var viewModel = new ChangePasswordViewModel();
            var viewName = "ChangePassword";

            BeforeChangePassword(viewModel, ref handled);
            if (handled == false)
            {
                if (SessionWrapper.LoginSession == null
                    || SessionWrapper.LoginSession.LogoutTime.HasValue)
                {
                    return RedirectToAction("Logon", new { returnUrl = "ChangePassword" });
                }
                viewModel.UserName = SessionWrapper.LoginSession.Name;
                viewModel.Email = SessionWrapper.LoginSession.Email;
            }
            AfterChangePassword(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        partial void BeforeChangePassword(ChangePasswordViewModel model, ref bool handled);
        partial void AfterChangePassword(ChangePasswordViewModel model, ref string viewName);

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(viewModel);
            }
            bool handled = false;
            var viewName = "ConfirmationChangePassword";

            BeforeDoChangePassword(viewModel, ref handled);
            if (handled == false)
            {
                if (SessionWrapper.LoginSession == null
                    || SessionWrapper.LoginSession.LogoutTime.HasValue)
                {
                    return RedirectToAction("Logon", new { returnUrl = "ChangePassword" });
                }

                try
                {
                    var accMngr = new AccountManager();

                    await accMngr.ChangePasswordAsync(SessionWrapper.LoginSession.SessionToken, viewModel.OldPassword, viewModel.NewPassword).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    viewModel.ActionError = GetExceptionError(ex);
                    return View("ChangePassword", viewModel);
                }
            }
            AfterDoChangePassword(viewModel, ref viewName);
            return View(viewName);
        }
        partial void BeforeDoChangePassword(ChangePasswordViewModel model, ref bool handled);
        partial void AfterDoChangePassword(ChangePasswordViewModel model, ref string viewName);

        public IActionResult ResetPassword()
        {
            var handled = false;
            var viewModel = new ResetPasswordViewModel();
            var viewName = "ResetPassword";

            BeforeResetPassword(viewModel, ref handled);
            if (handled == false)
            {
                if (SessionWrapper.LoginSession == null
                    || SessionWrapper.LoginSession.LogoutTime.HasValue)
                {
                    return RedirectToAction("Logon", new { returnUrl = "ChangePassword" });
                }
            }
            AfterResetPassword(viewModel, ref viewName);
            return View(viewName, viewModel);
        }
        partial void BeforeResetPassword(ResetPasswordViewModel model, ref bool handled);
        partial void AfterResetPassword(ResetPasswordViewModel model, ref string viewName);

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(viewModel);
            }
            bool handled = false;
            var viewName = "ConfirmationResetPassword";

            BeforeDoResetPassword(viewModel, ref handled);
            if (SessionWrapper.LoginSession == null
                || SessionWrapper.LoginSession.LogoutTime.HasValue)
            {
                return RedirectToAction("Logon", new { returnUrl = "ResetPassword" });
            }

            try
            {
                var accMngr = new AccountManager();

                await accMngr.ChangePasswordForAsync(SessionWrapper.SessionToken, viewModel.Email, viewModel.ConfirmPassword).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                viewModel.ActionError = GetExceptionError(ex);
                return View("ResetPassword", viewModel);
            }
            AfterDoResetPassword(viewModel, ref viewName);
            return View(viewName);
        }
        partial void BeforeDoResetPassword(ResetPasswordViewModel model, ref bool handled);
        partial void AfterDoResetPassword(ResetPasswordViewModel model, ref string viewName);

        private async Task ExecuteLogonAsync(LogonViewModel viewModel)
        {
            var intAccMngr = new AccountManager() { Adapter = Adapters.AdapterType.Controller };
            try
            {
                var internLogin = await intAccMngr.LogonAsync(viewModel.Email, viewModel.Password).ConfigureAwait(false);
                var loginSession = new LoginSession();

                loginSession.CopyProperties(internLogin);
                SessionWrapper.LoginSession = loginSession;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task ExecuteLogonRemoteAsync(LogonViewModel viewModel)
        {
            var intAccMngr = new AccountManager() { Adapter = Adapters.AdapterType.Controller };
            var extAccMngr = new AccountManager() { Adapter = Adapters.AdapterType.Service, BaseUri = viewModel.IdentityUrl };
            try
            {
                var externLogin = await extAccMngr.LogonAsync(viewModel.Email, viewModel.Password).ConfigureAwait(false);
                var internLogin = await intAccMngr.LogonAsync(externLogin.JsonWebToken).ConfigureAwait(false);
                var loginSession = new LoginSession();

                loginSession.CopyProperties(internLogin);
                SessionWrapper.LoginSession = loginSession;
                await extAccMngr.LogoutAsync(externLogin.SessionToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LogonRemote(LogonViewModel viewModel)
        {
            var intAccMngr = new AccountManager() { Adapter = Adapters.AdapterType.Controller };
            var extAccMngr = new AccountManager() { Adapter = Adapters.AdapterType.Service, BaseUri = viewModel.IdentityUrl };
            try
            {
                var externLogin = AsyncHelper.RunSync(() => extAccMngr.LogonAsync(viewModel.Email, viewModel.Password));
                var internLogin = AsyncHelper.RunSync(() => intAccMngr.LogonAsync(externLogin.JsonWebToken));
                var loginSession = new LoginSession();

                loginSession.CopyProperties(internLogin);
                SessionWrapper.LoginSession = loginSession;
                AsyncHelper.RunSync(() => extAccMngr.LogoutAsync(externLogin.SessionToken));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChangePasswordSync(ChangePasswordViewModel viewModel, string baseUri)
        {
            if (SessionWrapper.LoginSession == null
                || SessionWrapper.LoginSession.LogoutTime.HasValue == false)
            {
                var intAccMngr = new AccountManager() { Adapter = Adapters.AdapterType.Controller };
                var extAccMngr = new AccountManager() { Adapter = Adapters.AdapterType.Service, BaseUri = baseUri };
                try
                {
                    AsyncHelper.RunSync(() => intAccMngr.ChangePasswordAsync(SessionWrapper.SessionToken, viewModel.OldPassword, viewModel.NewPassword));
                    var externLogin = AsyncHelper.RunSync(() => extAccMngr.LogonAsync(viewModel.Email, viewModel.OldPassword));
                    AsyncHelper.RunSync(() => extAccMngr.ChangePasswordAsync(externLogin.SessionToken, viewModel.OldPassword, viewModel.NewPassword));
                    AsyncHelper.RunSync(() => extAccMngr.LogoutAsync(externLogin.SessionToken));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
//MdEnd
