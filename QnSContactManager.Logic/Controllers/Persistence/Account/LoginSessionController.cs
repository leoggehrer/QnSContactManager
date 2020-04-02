//@QnSCodeCopy
//MdStart
using System;
using System.Threading.Tasks;
using QnSContactManager.Logic.Entities.Persistence.Account;

namespace QnSContactManager.Logic.Controllers.Persistence.Account
{
    partial class LoginSessionController
    {
        protected override Task BeforeInsertingAsync(LoginSession entity)
        {
            entity.LoginTime = DateTime.Now;
            entity.LastAccess = entity.LoginTime;
            entity.SessionToken = Guid.NewGuid().ToString();
            return base.BeforeInsertingAsync(entity);
        }
    }
}
//MdEnd
