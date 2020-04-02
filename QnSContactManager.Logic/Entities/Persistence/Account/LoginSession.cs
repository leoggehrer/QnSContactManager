//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;

namespace QnSContactManager.Logic.Entities.Persistence.Account
{
    partial class LoginSession
    {
        partial void OnOriginReading()
        {
            _origin = nameof(QnSContactManager);
        }
        partial void OnLastAccessChanged()
        {
            HasChanged = true;
        }

        #region Ignore properties
        internal bool IsActive => IsTimeout == false;
        internal bool IsTimeout
        {
            get
            {
                TimeSpan ts = DateTime.Now - LastAccess;

                return LogoutTime.HasValue || ts.TotalSeconds > Logic.Modules.Security.Authorization.TimeOutInSec;
            }
        }
        internal bool HasChanged { get; set; }
        internal byte[] PasswordHash => Identity?.PasswordHash;
        internal List<Role> Roles { get; } = new List<Role>();
        #endregion Ignore properties
    }
}
//MdEnd
