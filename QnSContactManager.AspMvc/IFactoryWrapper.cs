//@QnSCodeCopy
//MdStart
using QnSContactManager.Contracts;
using QnSContactManager.Contracts.Client;

namespace QnSContactManager.AspMvc
{
    public interface IFactoryWrapper
    {
        IAdapterAccess<I> Create<I>() where I : IIdentifiable;
        IAdapterAccess<I> Create<I>(string sessionToken) where I : IIdentifiable;
    }
}
//MdEnd
