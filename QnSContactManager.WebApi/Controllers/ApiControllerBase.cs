//@QnSCodeCopy
//MdStart
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using CommonBase.Extensions;
using CommonBase.Helpers;

namespace QnSContactManager.WebApi.Controllers
{
    public abstract partial class ApiControllerBase : ControllerBase
    {
        static ApiControllerBase()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        protected ApiControllerBase()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public static string GetSessionToken(string authHeader)
        {
            string result = string.Empty;

            if (authHeader.HasContent())
            {
                if (authHeader.StartsWith("Bearer"))
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var encodedToken = authHeader.Substring("Bearer ".Length).Trim();

                    result = encoding.GetString(Convert.FromBase64String(encodedToken));
                }
                else if (authHeader.StartsWith("Basic"))
                {
                    string encodedUseridPassword = authHeader.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string useridPassword = encoding.GetString(Convert.FromBase64String(encodedUseridPassword));

                    int seperatorIndex = useridPassword.IndexOf(':');
                    var userid = useridPassword.Substring(0, seperatorIndex);
                    var password = useridPassword.Substring(seperatorIndex + 1);
                    var login = AsyncHelper.RunSync(() => Logic.Modules.Account.AccountManager.LogonAsync(userid, password));

                    result = login.SessionToken;
                }
            }
            return result;
        }
    }
}
//MdEnd
