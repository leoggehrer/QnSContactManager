namespace QnSContactManager.Logic.Controllers.Persistence.App
{
	sealed partial class ContactController : GenericController<QnSContactManager.Contracts.Persistence.App.IContact, Entities.Persistence.App.Contact>
	{
		static ContactController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();
		internal ContactController(DataContext.IContext context):base(context)
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		internal ContactController(ControllerObject controller):base(controller)
		{
			Constructing();
			Constructed();
		}
	}
}
namespace QnSContactManager.Logic.Controllers.Persistence.Account
{
	sealed partial class ActionLogController : GenericController<QnSContactManager.Contracts.Persistence.Account.IActionLog, Entities.Persistence.Account.ActionLog>
	{
		static ActionLogController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();
		internal ActionLogController(DataContext.IContext context):base(context)
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		internal ActionLogController(ControllerObject controller):base(controller)
		{
			Constructing();
			Constructed();
		}
	}
}
namespace QnSContactManager.Logic.Controllers.Persistence.Account
{
	[Logic.Modules.Security.Authorize("SysAdmin")]
	sealed partial class IdentityController : GenericController<QnSContactManager.Contracts.Persistence.Account.IIdentity, Entities.Persistence.Account.Identity>
	{
		static IdentityController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();
		internal IdentityController(DataContext.IContext context):base(context)
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		internal IdentityController(ControllerObject controller):base(controller)
		{
			Constructing();
			Constructed();
		}
	}
}
namespace QnSContactManager.Logic.Controllers.Persistence.Account
{
	[Logic.Modules.Security.Authorize("SysAdmin")]
	sealed partial class IdentityXRoleController : GenericController<QnSContactManager.Contracts.Persistence.Account.IIdentityXRole, Entities.Persistence.Account.IdentityXRole>
	{
		static IdentityXRoleController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();
		internal IdentityXRoleController(DataContext.IContext context):base(context)
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		internal IdentityXRoleController(ControllerObject controller):base(controller)
		{
			Constructing();
			Constructed();
		}
	}
}
namespace QnSContactManager.Logic.Controllers.Persistence.Account
{
	[Logic.Modules.Security.Authorize("SysAdmin")]
	sealed partial class LoginSessionController : GenericController<QnSContactManager.Contracts.Persistence.Account.ILoginSession, Entities.Persistence.Account.LoginSession>
	{
		static LoginSessionController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();
		internal LoginSessionController(DataContext.IContext context):base(context)
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		internal LoginSessionController(ControllerObject controller):base(controller)
		{
			Constructing();
			Constructed();
		}
	}
}
namespace QnSContactManager.Logic.Controllers.Persistence.Account
{
	[Logic.Modules.Security.Authorize("SysAdmin")]
	sealed partial class RoleController : GenericController<QnSContactManager.Contracts.Persistence.Account.IRole, Entities.Persistence.Account.Role>
	{
		static RoleController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();
		internal RoleController(DataContext.IContext context):base(context)
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		internal RoleController(ControllerObject controller):base(controller)
		{
			Constructing();
			Constructed();
		}
	}
}
