//@QnSCodeCopy
//MdStart
using System;

namespace QnSContactManager.Contracts.Persistence.Account
{
    public partial interface ILoginSession : IIdentifiable, ICopyable<ILoginSession>
    {
        int IdentityId { get; }
        bool IsRemoteAuth { get; }
        string Origin { get; }
        string Name { get; }
        string Email { get; }
        string JsonWebToken { get; }
        string SessionToken { get; }
        DateTime LoginTime { get; }
        DateTime LastAccess { get; }
        DateTime? LogoutTime { get; }
    }
}
//MdEnd
