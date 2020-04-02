//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CommonBase.Extensions;
using QnSContactManager.Contracts.Persistence.Account;
using QnSContactManager.Logic.Entities.Persistence.Account;
using QnSContactManager.Logic.Exceptions;
using QnSContactManager.Logic.Modules.Security;

namespace QnSContactManager.Logic.Modules.Account
{
    public static partial class AccountManager
    {
        static AccountManager()
        {
            ClassConstructing();
            Thread updateThread = new Thread(UpdateSession)
            {
                IsBackground = true
            };
            updateThread.Start();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private const int UpdateDelay = 60000;

        internal static readonly List<LoginSession> LoginSessions = new List<LoginSession>();

        #region Public logon
        public static async Task InitAppAccessAsync(string name, string email, string password, bool enableJwtAuth)
        {
            using var appAccessCtrl = new Controllers.Business.Account.AppAccessController(Factory.CreateContext())
            {
                SessionToken = Authorization.SystemAuthorizationToken,
            };

            var appAccessCount = await appAccessCtrl.CountAsync().ConfigureAwait(false);

            if (appAccessCount == 0)
            {
                var appAccess = await appAccessCtrl.CreateAsync().ConfigureAwait(false);

                appAccess.Identity.Name = name;
                appAccess.Identity.Email = email;
                appAccess.Identity.Password = password;
                appAccess.Identity.EnableJwtAuth = enableJwtAuth;
                var role = appAccess.CreateRole();

                role.Designation = "SysAdmin";
                appAccess.AddRole(role);
                await appAccessCtrl.InsertAsync(appAccess).ConfigureAwait(false);
                await appAccessCtrl.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                throw new LogicException(ErrorType.InitAppAccess);
            }
        }
        public async static Task<ILoginSession> LogonAsync(string jsonWebToken)
        {
            jsonWebToken.CheckArgument(nameof(jsonWebToken));

            var result = default(LoginSession);

            if (JsonWebToken.CheckToken(jsonWebToken, out SecurityToken validatedToken))
            {
                if (validatedToken.ValidTo < DateTime.UtcNow)
                    throw new LogicException(ErrorType.AuthorizationTimeOut);

                var jwtValidatedToken = validatedToken as JwtSecurityToken;

                if (jwtValidatedToken != null)
                {
                    var email = jwtValidatedToken.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email);

                    if (email != null && email.Value != null)
                    {
                        using var identityCtrl = new Controllers.Persistence.Account.IdentityController(Factory.CreateContext())
                        {
                            SessionToken = Authorization.SystemAuthorizationToken
                        };
                        var identity = identityCtrl.ExecuteQuery(e => e.State == Contracts.Modules.Common.State.Active
                                                            && e.EnableJwtAuth == true
                                                            && e.Email.ToLower() == email.Value.ToString().ToLower())
                                                   .ToList()
                                                   .FirstOrDefault();

                        if (identity != null)
                        {
                            var login = await QueryLoginAsync(identity.Email, identity.PasswordHash).ConfigureAwait(false);

                            if (login != null)
                            {
                                result = new LoginSession();
                                result.CopyProperties(login);
                                result.IsRemoteAuth = true;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new LogicException(ErrorType.InvalidJsonWebToken);
            }
            return result ?? throw new LogicException(ErrorType.InvalidAccount);
        }
        public static async Task<ILoginSession> LogonAsync(string email, string password)
        {
            var result = default(ILoginSession);
            var calculatedHash = CalculateHash(password);
            var login = await QueryLoginAsync(email, calculatedHash).ConfigureAwait(false);

            if (login != null)
            {
                result = new LoginSession();
                result.CopyProperties(login);
            }
            return result ?? throw new LogicException(ErrorType.InvalidAccount);
        }
        [Authorize]
        public static async Task LogoutAsync(string sessionToken)
        {
            Authorization.CheckAuthorization(sessionToken, MethodBase.GetCurrentMethod());

            try
            {
                using var sessionCtrl = new Controllers.Persistence.Account.LoginSessionController(Factory.CreateContext())
                {
                    SessionToken = Authorization.SystemAuthorizationToken
                };
                var session = sessionCtrl.ExecuteQuery(e => e.SessionToken.Equals(sessionToken))
                                         .ToList()
                                         .FirstOrDefault(e => e.IsActive);

                if (session != null)
                {
                    session.LogoutTime = DateTime.Now;

                    await sessionCtrl.UpdateAsync(session).ConfigureAwait(false);
                    await sessionCtrl.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (LogicException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }
        [Authorize]
        public static async Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            Authorization.CheckAuthorization(sessionToken, MethodBase.GetCurrentMethod());

            role.CheckArgument(nameof(role));

            var loginSession = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);

            return loginSession != null && loginSession.Roles.Any(r => r.Designation.Equals(role, StringComparison.CurrentCultureIgnoreCase));
        }
        [Authorize]
        public static async Task<ILoginSession> QueryLoginAsync(string sessionToken)
        {
            Authorization.CheckAuthorization(sessionToken, MethodBase.GetCurrentMethod());

            return await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);
        }
        [Authorize]
        public static async Task ChangePasswordAsync(string sessionToken, string oldPassword, string newPassword)
        {
            Authorization.CheckAuthorization(sessionToken, MethodBase.GetCurrentMethod());

            var login = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false)
                        ?? throw new LogicException(ErrorType.InvalidToken);

            using var identityCtrl = new Controllers.Persistence.Account.IdentityController(Factory.CreateContext())
            {
                SessionToken = Authorization.SystemAuthorizationToken
            };
            var identity = identityCtrl.ExecuteQueryById(login.IdentityId);

            if (identity != null)
            {
                if (ComparePasswords(identity.PasswordHash, CalculateHash(oldPassword)) == false)
                    throw new LogicException(ErrorType.InvalidPassword);

                identity.Password = newPassword;
                await identityCtrl.UpdateAsync(identity).ConfigureAwait(false);
                await identityCtrl.SaveChangesAsync().ConfigureAwait(false);
                if (login.Identity != null)
                {
                    login.Identity.PasswordHash = CalculateHash(newPassword);
                }
            }
        }
        [Authorize("SysAdmin")]
        public static async Task ChangePasswordForAsync(string sessionToken, string email, string newPassword)
        {
            Authorization.CheckAuthorization(sessionToken, MethodBase.GetCurrentMethod());

            var login = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false)
                        ?? throw new LogicException(ErrorType.InvalidToken);

            using var identityCtrl = new Controllers.Persistence.Account.IdentityController(Factory.CreateContext())
            {
                SessionToken = sessionToken
            };
            var identity = identityCtrl.ExecuteQuery(e => e.State == Contracts.Modules.Common.State.Active
                                                && e.AccessFailedCount < 4
                                                && e.Email.ToLower() == email.ToLower())
                                       .FirstOrDefault();

            if (identity == null)
                throw new LogicException(ErrorType.InvalidAccount);


            identity.AccessFailedCount = 0;
            identity.Password = newPassword;
            await identityCtrl.UpdateAsync(identity).ConfigureAwait(false);
            await identityCtrl.SaveChangesAsync().ConfigureAwait(false);
            if (login.Identity != null)
            {
                login.Identity.PasswordHash = CalculateHash(newPassword);
            }
        }
        [Authorize("SysAdmin")]
        public static async Task ResetForAsync(string sessionToken, string email)
        {
            Authorization.CheckAuthorization(sessionToken, MethodBase.GetCurrentMethod());

            var login = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false)
                        ?? throw new LogicException(ErrorType.InvalidToken);

            using var identityCtrl = new Controllers.Persistence.Account.IdentityController(Factory.CreateContext())
            {
                SessionToken = sessionToken
            };
            var identity = identityCtrl.ExecuteQuery(e => e.State == Contracts.Modules.Common.State.Active
                                                && e.Email.ToLower() == email.ToLower())
                                       .FirstOrDefault();

            if (identity == null)
                throw new LogicException(ErrorType.InvalidAccount);


            identity.AccessFailedCount = 0;
            await identityCtrl.UpdateAsync(identity).ConfigureAwait(false);
            await identityCtrl.SaveChangesAsync().ConfigureAwait(false);
        }
        #endregion Public logon

        #region Internal logon
        internal static async Task<LoginSession> QueryAliveSessionAsync(string sessionToken)
        {
            LoginSession result = LoginSessions.FirstOrDefault(ls => ls.SessionToken.Equals(sessionToken));

            if (result == null)
            {
                using var sessionCtrl = new Controllers.Persistence.Account.LoginSessionController(Factory.CreateContext())
                {
                    SessionToken = Authorization.SystemAuthorizationToken
                };
                var session = sessionCtrl.ExecuteQuery(e => e.SessionToken.Equals(sessionToken))
                                         .ToList()
                                         .FirstOrDefault(e => e.IsActive);

                if (session != null)
                {
                    using var identityCtrl = new Controllers.Persistence.Account.IdentityController(sessionCtrl);
                    var identity = identityCtrl.ExecuteQuery(e => e.Id == session.IdentityId).FirstOrDefault();

                    if (identity != null)
                    {
                        result = new LoginSession();
                        result.CopyProperties(session);
                        result.Identity = new Identity();
                        result.Identity.CopyProperties(identity);
                        result.Name = identity.Name;
                        result.Email = identity.Email;
                        result.Roles.AddRange(await QueryIdentityRolesAsync(sessionCtrl, identity.Id).ConfigureAwait(false));
                        result.JsonWebToken = JsonWebToken.GenerateToken(new Claim[]
                        {
                            new Claim(ClaimTypes.Email, identity.Email),
                        }.Union(result.Roles.Select(e => new Claim(ClaimTypes.Role, e.Designation))));
                        LoginSessions.Add(result);
                    }
                }
            }
            return result;
        }
        internal static async Task<LoginSession> QueryLoginAsync(string email, byte[] calculatedHash)
        {
            email.CheckArgument(nameof(email));
            calculatedHash.CheckArgument(nameof(calculatedHash));

            var result = await QueryAliveSessionAsync(email, calculatedHash).ConfigureAwait(false);

            if (result == null)
            {
                using var identityCtrl = new Controllers.Persistence.Account.IdentityController(Factory.CreateContext())
                {
                    SessionToken = Authorization.SystemAuthorizationToken,
                };
                var identity = identityCtrl.ExecuteQuery(e => e.State == Contracts.Modules.Common.State.Active
                                                && e.AccessFailedCount < 4
                                                && e.Email.ToLower() == email.ToLower()
                                                && e.PasswordHash == calculatedHash).FirstOrDefault();

                if (identity != null)
                {
                    using var sessionCtrl = new Controllers.Persistence.Account.LoginSessionController(identityCtrl);
                    var session = new LoginSession();

                    session.IdentityId = identity.Id;
                    session.Name = identity.Name;
                    session.Email = identity.Email;
                    session.Roles.AddRange(await QueryIdentityRolesAsync(sessionCtrl, identity.Id).ConfigureAwait(false));
                    var entity = await sessionCtrl.ExecuteInsertAsync(session).ConfigureAwait(false);

                    if (identity.AccessFailedCount > 0)
                    {
                        identity.AccessFailedCount = 0;
                        await identityCtrl.ExecuteUpdateAsync(identity).ConfigureAwait(false);
                    }
                    await sessionCtrl.SaveChangesAsync().ConfigureAwait(false);

                    result = new LoginSession();
                    result.CopyProperties(session);
                    result.Name = identity.Name;
                    result.Email = identity.Email;
                    result.Roles.AddRange(session.Roles);
                    result.JsonWebToken = JsonWebToken.GenerateToken(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, identity.Email),
                        new Claim(ClaimTypes.System, nameof(QnSContactManager)),
                    }.Union(result.Roles.Select(e => new Claim(ClaimTypes.Role, e.Designation))));
                    LoginSessions.Add(result);
                }
            }
            return result;
        }
        internal static async Task<LoginSession> QueryAliveSessionAsync(string email, byte[] calculatedHash)
        {
            email.CheckArgument(nameof(email));
            calculatedHash.CheckArgument(nameof(calculatedHash));

            LoginSession result = LoginSessions.FirstOrDefault(e => e.IsActive
                                                                 && e.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)
                                                                 && e.PasswordHash == calculatedHash);

            if (result == null)
            {
                using var identityCtrl = new Controllers.Persistence.Account.IdentityController(Factory.CreateContext())
                {
                    SessionToken = Authorization.SystemAuthorizationToken,
                };
                var identity = identityCtrl.ExecuteQuery(e => e.State == Contracts.Modules.Common.State.Active
                                                    && e.AccessFailedCount < 4
                                                    && e.Email.ToLower() == email.ToLower()
                                                    && e.PasswordHash == calculatedHash).FirstOrDefault();

                if (identity != null)
                {
                    using var sessionCtrl = new Controllers.Persistence.Account.LoginSessionController(identityCtrl);
                    var session = sessionCtrl.ExecuteQuery(e => e.LogoutTime == null
                                                      && e.IdentityId == identity.Id)
                                             .ToList()
                                             .FirstOrDefault(e => e.IsActive);

                    if (session != null)
                    {
                        result = new LoginSession();
                        result.CopyProperties(session);
                        result.Identity = new Identity();
                        result.Identity.CopyProperties(identity);
                        result.Name = identity.Name;
                        result.Email = identity.Email;
                        result.Roles.AddRange(await QueryIdentityRolesAsync(sessionCtrl, identity.Id).ConfigureAwait(false));
                        result.JsonWebToken = JsonWebToken.GenerateToken(new Claim[]
                        {
                            new Claim(ClaimTypes.Email, identity.Email),
                        }.Union(result.Roles.Select(e => new Claim(ClaimTypes.Role, e.Designation))));
                        LoginSessions.Add(result);
                    }
                }
            }
            return result;
        }
        internal static async Task<IEnumerable<Role>> QueryIdentityRolesAsync(Controllers.ControllerObject controllerObject, int identityId)
        {
            controllerObject.CheckArgument(nameof(controllerObject));

            List<Role> result = new List<Role>();
            using var identityXRoleCtrl = new Controllers.Persistence.Account.IdentityXRoleController(controllerObject);
            using var roleCtrl = new Controllers.Persistence.Account.RoleController(controllerObject);

            foreach (var item in identityXRoleCtrl.ExecuteQuery(e => e.IdentityId == identityId).ToList())
            {
                var entity = await roleCtrl.ExecuteGetByIdAsync(item.RoleId).ConfigureAwait(false);

                if (entity != null)
                {
                    var role = new Role();

                    role.CopyProperties(entity);
                    result.Add(role);
                }
            }
            return result;
        }
        #endregion Internal logon

        #region Update thread
        private static void UpdateSession()
        {
            while (true)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        using var sessionCtrl = new Controllers.Persistence.Account.LoginSessionController(Factory.CreateContext())
                        {
                            SessionToken = Authorization.SystemAuthorizationToken,
                        };
                        bool saveChanges = false;
                        var dbSessions = sessionCtrl.ExecuteQuery(e => e.LogoutTime.HasValue == false).ToList();
                        var uncheckSessions = LoginSessions.Where(i => dbSessions.Any() == false 
                                                                    || dbSessions.Any(e => e.Id != i.Id));

                        foreach (var dbItem in dbSessions)
                        {
                            var itemUpdate = false;
                            var memItemRemove= false;
                            var memItem = LoginSessions.FirstOrDefault(e => e.Id == dbItem.Id);

                            if (memItem != null && memItem.HasChanged)
                            {
                                itemUpdate = true;
                                memItem.HasChanged = false;
                                dbItem.LastAccess = memItem.LastAccess;
                            }
                            if (dbItem.IsTimeout)
                            {
                                itemUpdate = true;
                                if (memItem != null)
                                {
                                    memItemRemove = true;
                                }
                                if (dbItem.LogoutTime.HasValue == false)
                                {
                                    dbItem.LogoutTime = DateTime.Now;
                                }
                            }
                            if (itemUpdate)
                            {
                                saveChanges = true;
                                await sessionCtrl.ExecuteUpdateAsync(dbItem).ConfigureAwait(false);
                            }
                            if (memItemRemove)
                            {
                                LoginSessions.Remove(memItem);
                            }
                        }
                        if (saveChanges)
                        {
                            await sessionCtrl.SaveChangesAsync().ConfigureAwait(false);
                        }
                        foreach (var memItem in uncheckSessions)
                        {
                            var dbItem = sessionCtrl.ExecuteQueryById(memItem.Id);

                            if (dbItem != null)
                            {
                                memItem.LastAccess = dbItem.LastAccess;
                                memItem.LogoutTime = dbItem.LogoutTime;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                    }
                });
                Thread.Sleep(UpdateDelay);
            }
        }
        #endregion Update thread

        #region Helpers
        internal static byte[] CalculateHash(string plainText)
        {
            if (String.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] hashedBytes = sha1.ComputeHash(plainTextBytes);
            return hashedBytes;
        }
        internal static bool ComparePasswords(Byte[] passwordHash, string password)
        {
            return ComparePasswords(passwordHash, CalculateHash(password));
        }
        internal static bool ComparePasswords(Byte[] passwordHash, Byte[] calculatedHash)
        {
            if (passwordHash == null)
                throw new ArgumentNullException(nameof(passwordHash));

            if (calculatedHash == null)
                throw new ArgumentNullException(nameof(calculatedHash));

            byte[] originalArray = passwordHash.ToArray();
            byte[] compareArray = calculatedHash.ToArray();

            if (compareArray.Length != originalArray.Length)
            {
                return false;
            }
            for (int i = 0; i < compareArray.Length; i++)
            {
                if (originalArray[i] != compareArray[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Das Kennwort wenn es den Einstellungen im CommonBase/Modules/PasswordRules entspricht.
        /// </summary>
        /// <param name="password">Zu pruefendes Passwort</param>
        /// <returns>true wenn das Passwort mit PasswordRules entspricht, false sonst</returns>
        public static bool CheckPasswordSyntax(string password)
        {
            password.CheckArgument(nameof(password));

            int digitCount = 0;
            int letterCount = 0;
            int lowerLetterCount = 0;
            int upperLetterCount = 0;
            int specialLetterCount = 0;

            foreach (char ch in password)
            {
                if (char.IsDigit(ch))
                {
                    digitCount++;
                }
                else
                {
                    if (char.IsLetter(ch))
                    {
                        letterCount++;
                        if (char.IsLower(ch))
                        {
                            lowerLetterCount++;
                        }
                        else
                        {
                            upperLetterCount++;
                        }
                    }
                    else
                    {
                        specialLetterCount++;
                    }
                }
            }
            return password.Length >= PasswordRules.MinimumLength
                   && password.Length <= PasswordRules.MaximumLength
                   && letterCount >= PasswordRules.MinLetterCount
                   && upperLetterCount >= PasswordRules.MinUpperLetterCount
                   && lowerLetterCount >= PasswordRules.MinLowerLetterCount
                   && specialLetterCount >= PasswordRules.MinSpecialLetterCount
                   && digitCount >= PasswordRules.MinDigitCount;
        }

        /// <summary>
        /// Eine gueltige Mailadresse besteht aus einem mindestens zwei Zeichen vor dem @, 
        /// einem Hostname, der genau einen oder mehrere Punkte enthaelt (Domainname mindestens dreistellig)
        /// und als Topleveldomaene (letzter Teil) mindestens zweistellig ist
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns>Mailadresse ist gültig</returns>
        public static bool CheckMailAddressSyntax(string mailAddress)
        {
            mailAddress.CheckArgument(nameof(mailAddress));

            //return Regex.IsMatch(mailAddress, @"^([\w-\.]+){2,}@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            ////@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
            ////@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"); 
            return Regex.IsMatch(mailAddress, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            //return Regex.IsMatch(mailAddress, @"^\w{2,}@[a-zA-Z]{3,}\.[a-zA-Z]{2,}$");
        }
        #endregion Helpers
    }
}
//MdEnd
