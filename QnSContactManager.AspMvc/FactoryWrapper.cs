//@QnSCodeCopy
//MdStart
namespace QnSContactManager.AspMvc
{
    public partial class FactoryWrapper : IFactoryWrapper
    {
        public Contracts.Client.IAdapterAccess<I> Create<I>() where I : Contracts.IIdentifiable
        {
            return Adapters.Factory.Create<I>();
        }
        public Contracts.Client.IAdapterAccess<I> Create<I>(string sessionToken) where I : Contracts.IIdentifiable
        {
            return Adapters.Factory.Create<I>(sessionToken);
        }
    }
}
//MdEnd
