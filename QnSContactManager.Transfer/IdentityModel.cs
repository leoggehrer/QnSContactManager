//@QnSCodeCopy
//MdStart

namespace QnSContactManager.Transfer
{
    public abstract partial class IdentityModel : TransferModel, Contracts.IIdentifiable
    {
        public virtual int Id { get; set; }
        public virtual byte[] Timestamp { get; set; }
	}
}
//MdEnd
