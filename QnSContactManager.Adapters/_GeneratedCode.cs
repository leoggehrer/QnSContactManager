namespace QnSContactManager.Adapters
{
	public static partial class Factory
	{
		public static Contracts.Client.IAdapterAccess<I> Create<I>() where I : Contracts.IIdentifiable
		{
			Contracts.Client.IAdapterAccess<I> result = null;
			if (Adapter == AdapterType.Controller)
			{
				if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.App.IContact))
				{
					result = new Controller.GenericControllerAdapter<QnSContactManager.Contracts.Persistence.App.IContact>() as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IRole))
				{
					result = new Controller.GenericControllerAdapter<QnSContactManager.Contracts.Persistence.Account.IRole>() as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Business.Account.IAppAccess))
				{
					result = new Controller.GenericControllerAdapter<QnSContactManager.Contracts.Business.Account.IAppAccess>() as Contracts.Client.IAdapterAccess<I>;
				}
			}
			else if (Adapter == AdapterType.Service)
			{
				if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.App.IContact))
				{
					result = new Service.GenericServiceAdapter<QnSContactManager.Contracts.Persistence.App.IContact, Transfer.Persistence.App.Contact>(BaseUri, "Contact") as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IRole))
				{
					result = new Service.GenericServiceAdapter<QnSContactManager.Contracts.Persistence.Account.IRole, Transfer.Persistence.Account.Role>(BaseUri, "Role") as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Business.Account.IAppAccess))
				{
					result = new Service.GenericServiceAdapter<QnSContactManager.Contracts.Business.Account.IAppAccess, Transfer.Business.Account.AppAccess>(BaseUri, "AppAccess") as Contracts.Client.IAdapterAccess<I>;
				}
			}
			return result;
		}
		public static Contracts.Client.IAdapterAccess<I> Create<I>(string sessionToken) where I : Contracts.IIdentifiable
		{
			Contracts.Client.IAdapterAccess<I> result = null;
			if (Adapter == AdapterType.Controller)
			{
				if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.App.IContact))
				{
					result = new Controller.GenericControllerAdapter<QnSContactManager.Contracts.Persistence.App.IContact>(sessionToken) as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IRole))
				{
					result = new Controller.GenericControllerAdapter<QnSContactManager.Contracts.Persistence.Account.IRole>(sessionToken) as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Business.Account.IAppAccess))
				{
					result = new Controller.GenericControllerAdapter<QnSContactManager.Contracts.Business.Account.IAppAccess>(sessionToken) as Contracts.Client.IAdapterAccess<I>;
				}
			}
			else if (Adapter == AdapterType.Service)
			{
				if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.App.IContact))
				{
					result = new Service.GenericServiceAdapter<QnSContactManager.Contracts.Persistence.App.IContact, Transfer.Persistence.App.Contact>(sessionToken, BaseUri, "Contact") as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Persistence.Account.IRole))
				{
					result = new Service.GenericServiceAdapter<QnSContactManager.Contracts.Persistence.Account.IRole, Transfer.Persistence.Account.Role>(sessionToken, BaseUri, "Role") as Contracts.Client.IAdapterAccess<I>;
				}
				else if (typeof(I) == typeof(QnSContactManager.Contracts.Business.Account.IAppAccess))
				{
					result = new Service.GenericServiceAdapter<QnSContactManager.Contracts.Business.Account.IAppAccess, Transfer.Business.Account.AppAccess>(sessionToken, BaseUri, "AppAccess") as Contracts.Client.IAdapterAccess<I>;
				}
			}
			return result;
		}
	}
}
