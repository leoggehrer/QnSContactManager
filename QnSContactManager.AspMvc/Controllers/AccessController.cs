//@QnSCodeCopy
//MdStart
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QnSContactManager.AspMvc.Modules.Session;
using AccountManager = QnSContactManager.Adapters.Modules.Account.AccountManager;

namespace QnSContactManager.AspMvc.Controllers
{
    public abstract partial class AccessController : MvcController
    {
        static AccessController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        protected AccessController(IFactoryWrapper factoryWrapper)
            : base(factoryWrapper)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            static string GetReturnUrlFromPath(PathString path)
            {
                string result;
                string[] data = path.ToString().Split('/');

                if (data.Length > 1)
                {
                    result = $"/{data[1]}";
                }
                else
                {
                    result = string.Empty;
                }
                return result;
            }

            SessionWrapper sessionWrapper = new SessionWrapper(context.HttpContext.Session);

            if (sessionWrapper.LoginSession == null)
            {
                context.Result = ((Controller)context.Controller).RedirectToAction("Logon", "Account", new { returnUrl = context.HttpContext.Request.Path });
            }
            else
            {
                try
                {
                    AccountManager accMngr = new AccountManager();
                    var loginSession = CommonBase.Helpers.AsyncHelper.RunSync(() => accMngr.QueryLoginAsync(sessionWrapper.SessionToken));

                    if (loginSession == null)
                    {
                        string error = "Session timeout!";
                        string returnUrl = GetReturnUrlFromPath(context.HttpContext.Request.Path);

                        context.Result = ((Controller)context.Controller).RedirectToAction("Logon", "Account", new { returnUrl, error });
                    }
                }
                catch (Exception ex)
                {
                    string error = GetExceptionError(ex);
                    string returnUrl = GetReturnUrlFromPath(context.HttpContext.Request.Path);

                    sessionWrapper.LoginSession = null;
                    context.Result = ((Controller)context.Controller).RedirectToAction("Logon", "Account", new { returnUrl, error });
                }
            }
        }
    }
}
//MdEnd
