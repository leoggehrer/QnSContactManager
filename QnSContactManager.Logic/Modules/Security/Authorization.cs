//@QnSCodeCopy
//MdStart
using System;
using System.Linq;
using System.Reflection;
using CommonBase.Helpers;
using CommonBase.Extensions;
using QnSContactManager.Logic.Entities.Persistence.Account;
using QnSContactManager.Logic.Modules.Account;
using System.Threading.Tasks;
using QnSContactManager.Logic.Exceptions;

namespace QnSContactManager.Logic.Modules.Security
{
    internal static partial class Authorization
    {
        static Authorization()
        {
            ClassConstructing();
            if (SystemAuthorizationToken.IsNullOrEmpty())
            {
                SystemAuthorizationToken = Guid.NewGuid().ToString();
            }
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        internal static int TimeOutInMin { get; private set; } = 30;
        internal static int TimeOutInSec => TimeOutInMin * 60;
        internal static string SystemAuthorizationToken { get; set; }

        internal static void CheckAuthorization(string sessionToken, MethodBase methodeBase)
        {
            sessionToken.CheckArgument(nameof(sessionToken));
            methodeBase.CheckArgument(nameof(methodeBase));

            bool handled = false;

            BeforeCheckAuthorization(sessionToken, methodeBase, ref handled);
            if (handled == false)
            {
                CheckAuthorizationInternal(sessionToken, methodeBase);
            }
            AfterCheckAuthorization(sessionToken, methodeBase);
        }
        private static void CheckAuthorizationInternal(string sessionToken, MethodBase methodBase)
        {
            methodBase = methodBase.GetOriginal();
            if (sessionToken.IsNullOrEmpty())
            {
                var authorization = methodBase.GetCustomAttribute<AuthorizeAttribute>();
                var isRequired = authorization?.IsRequired ?? false;

                if (isRequired)
                    throw new AuthorizationException(ErrorType.NotLogedIn);
            }
            else if (sessionToken.Equals(SystemAuthorizationToken) == false)
            {
                var authorization = methodBase.GetCustomAttribute<AuthorizeAttribute>();
                bool isRequired = authorization?.IsRequired ?? false;

                if (isRequired)
                {
                    var curSession = AsyncHelper.RunSync<LoginSession>(() => AccountManager.QueryAliveSessionAsync(sessionToken));

                    if (curSession == null)
                        throw new AuthorizationException(ErrorType.InvalidSessionToken);

                    if (curSession.IsTimeout)
                        throw new AuthorizationException(ErrorType.AuthorizationTimeOut);

                    bool isAuthorized = authorization.Roles.Any() == false
                                        || curSession.Roles.Any(lr => authorization.Roles.Contains(lr.Designation));

                    if (isAuthorized == false)
                        throw new AuthorizationException(ErrorType.NotAuthorized);

                    curSession.LastAccess = DateTime.Now;
                    LoggingAsync(curSession.IdentityId, methodBase.DeclaringType.Name, methodBase.Name, string.Empty);
                }
            }
        }
        static partial void BeforeCheckAuthorization(string sessionToken, MethodBase methodeBase, ref bool handled);
        static partial void AfterCheckAuthorization(string sessionToken, MethodBase methodeBase);

        internal static void CheckAuthorization(string sessionToken, Type instanceType, MethodBase methodeBase)
        {
            bool handled = false;

            BeforeCheckAuthorization(sessionToken, instanceType, methodeBase, ref handled);
            if (handled == false)
            {
                CheckAuthorizationInternal(sessionToken, instanceType, methodeBase);
            }
            AfterCheckAuthorization(sessionToken, instanceType, methodeBase);
        }
        private static void CheckAuthorizationInternal(string sessionToken, Type instanceType, MethodBase methodBase)
        {
            static AuthorizeAttribute GetClassAuthorization(Type classType)
            {
                var runType = classType;
                var result = default(AuthorizeAttribute);

                do
                {
                    result = runType.GetCustomAttribute<AuthorizeAttribute>();
                    runType = runType.BaseType;
                } while (result == null && runType != null);
                return result;
            }

            methodBase = methodBase.GetOriginal();
            if (sessionToken.IsNullOrEmpty())
            {
                var authorization = methodBase.GetCustomAttribute<AuthorizeAttribute>()
                                    ?? GetClassAuthorization(instanceType);
                var isRequired = authorization?.IsRequired ?? false;

                if (isRequired)
                {
                    throw new AuthorizationException(ErrorType.NotLogedIn);
                }
            }
            else if (sessionToken.Equals(SystemAuthorizationToken) == false)
            {
                var authorization = methodBase.GetCustomAttribute<AuthorizeAttribute>()
                                    ?? GetClassAuthorization(instanceType);
                var isRequired = authorization?.IsRequired ?? false;

                if (isRequired)
                {
                    var curSession = AsyncHelper.RunSync<LoginSession>(() => AccountManager.QueryAliveSessionAsync(sessionToken));

                    if (curSession == null)
                        throw new AuthorizationException(ErrorType.InvalidSessionToken);

                    if (curSession.IsTimeout)
                        throw new AuthorizationException(ErrorType.AuthorizationTimeOut);

                    bool isAuthorized = authorization.Roles.Any() == false
                                        || curSession.Roles.Any(lr => authorization.Roles.Contains(lr.Designation));

                    if (isAuthorized == false)
                        throw new AuthorizationException(ErrorType.NotAuthorized);

                    curSession.LastAccess = DateTime.Now;
                    LoggingAsync(curSession.IdentityId, instanceType.Name, methodBase.Name, string.Empty);
                }
            }
        }

        static partial void BeforeCheckAuthorization(string sessionToken, Type instanceType, MethodBase methodeBase, ref bool handled);
        static partial void AfterCheckAuthorization(string sessionToken, Type instanceType, MethodBase methodeBase);

        static Task LoggingAsync(int identityId, string subject, string action, string info)
        {
            return Task.Run(async () =>
            {
                using var actionLogCtrl = new Logic.Controllers.Persistence.Account.ActionLogController(Factory.CreateContext())
                {
                    SessionToken = SystemAuthorizationToken
                };
                var entity = new ActionLog();

                entity.IdentityId = identityId;
                entity.Time = DateTime.Now;
                entity.Subject = subject;
                entity.Action = action;
                entity.Info = info;
                await actionLogCtrl.InsertAsync(entity).ConfigureAwait(false);
                await actionLogCtrl.SaveChangesAsync().ConfigureAwait(false);
            });
        }
    }
}
//MdEnd
