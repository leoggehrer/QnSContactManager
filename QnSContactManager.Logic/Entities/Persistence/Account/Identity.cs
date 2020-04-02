//@QnSCodeCopy
//MdStart

namespace QnSContactManager.Logic.Entities.Persistence.Account
{
    partial class Identity
    {
        public byte[] PasswordHash { get; set; }

        public void CopyProperties(Identity identity)
        {
            CopyProperties(identity as Contracts.Persistence.Account.IIdentity);

            PasswordHash = identity.PasswordHash;
        }
    }
}
//MdEnd
