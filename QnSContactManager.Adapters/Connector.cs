//@QnSCodeCopy
//MdStart

namespace QnSContactManager.Adapters
{
    public static partial class Connector
    {
        static Connector()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        /// <summary>
        /// The base url like https://localhost:5001/api
        /// </summary>
        public static string BaseUri { get; set; }

        public static Contracts.Client.IAdapterAccess<TContract> Create<TContract, TModel>() 
            where TContract : Contracts.IIdentifiable
            where TModel: TContract, Contracts.ICopyable<TContract>, new()
        {
            Contracts.Client.IAdapterAccess<TContract> result = null;

            result = new Service.GenericServiceAdapter<TContract, TModel>(BaseUri, typeof(TModel).Name) as Contracts.Client.IAdapterAccess<TContract>;
            return result;
        }
        public static Contracts.Client.IAdapterAccess<TContract> Create<TContract, TModel>(string sessionToken)
            where TContract : Contracts.IIdentifiable
            where TModel : TContract, Contracts.ICopyable<TContract>, new()
        {
            Contracts.Client.IAdapterAccess<TContract> result = null;

            result = new Service.GenericServiceAdapter<TContract, TModel>(sessionToken, BaseUri, typeof(TModel).Name) as Contracts.Client.IAdapterAccess<TContract>;
            return result;
        }
        public static Contracts.Client.IAdapterAccess<TContract> Create<TContract, TModel>(string baseUri, string sessionToken)
            where TContract : Contracts.IIdentifiable
            where TModel : TContract, Contracts.ICopyable<TContract>, new()
        {
            Contracts.Client.IAdapterAccess<TContract> result = null;

            result = new Service.GenericServiceAdapter<TContract, TModel>(sessionToken, baseUri, typeof(TModel).Name) as Contracts.Client.IAdapterAccess<TContract>;
            return result;
        }
    }
}
//MdEnd
