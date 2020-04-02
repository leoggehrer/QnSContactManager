namespace QnSContactManager.WebApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Contract = Contracts.Persistence.App.IContact;
	using Model = Transfer.Persistence.App.Contact;
	[ApiController]
	[Route("Controller")]
	public partial class ContactController : GenericController<Contract, Model>
	{
		[HttpGet("/api/[controller]/MaxPageSize")]
		public Task<int> GetMaxPageAsync()
		{
			return GetMaxPageAsync();
		}
		[HttpGet("/api/[controller]/Count")]
		public Task<int> GetCountAsync()
		{
			return CountModelsAsync();
		}
		[HttpGet("/api/[controller]/CountBy/{predicate}")]
		public Task<int> GetCountByAsync(string predicate)
		{
			return CountModelsByAsync(predicate);
		}
		[HttpGet("/api/[controller]/GetById/{id}")]
		public Task<Model> GetByIdAsync(int id)
		{
			return GetModelByIdAsync(id);
		}
		[HttpGet("/api/[controller]/GetPageList/{index}/{size}")]
		public Task<IEnumerable<Model>> GetPageListAsync(int index, int size)
		{
			return GetModelPageListAsync(index, size);
		}
		[HttpGet("/api/[controller]/GetAll")]
		public Task<IEnumerable<Model>> GetAllAsync()
		{
			return GetAllModelsAsync();
		}
		[HttpGet("/api/[controller]/QueryPageList/{predicate}/{index}/{size}")]
		public Task<IEnumerable<Model>> QueryPageListAsync(string predicate, int index, int size)
		{
			return QueryModelPageListAsync(predicate, index, size);
		}
		[HttpGet("/api/[controller]/QueryAll/{predicate}")]
		public Task<IEnumerable<Model>> QueryAllAsync(string predicate)
		{
			return QueryAllModelsAsync(predicate);
		}
		[HttpGet("/api/[controller]/Create")]
		public Task<Model> CreateAsync()
		{
			return CreateModelAsync();
		}
		[HttpPost("/api/[controller]")]
		public Task<Model> PostAsync(Model model)
		{
			return InsertModelAsync(model);
		}
		[HttpPut("/api/[controller]")]
		public Task<Model> PutAsync(Model model)
		{
			return UpdateModelAsync(model);
		}
		[HttpDelete("/api/[controller]/{id}")]
		public Task DeleteAsync(int id)
		{
			return DeleteModelAsync(id);
		}
		[HttpPost("/api/[controller]/CallAction")]
		public Task CallActionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeActionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
		[HttpPost("/api/[controller]/CallFunction")]
		public Task<Transfer.InvokeTypes.InvokeReturnValue> CallFunctionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeFunctionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
	}
}
namespace QnSContactManager.WebApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Contract = Contracts.Persistence.Account.IActionLog;
	using Model = Transfer.Persistence.Account.ActionLog;
	[ApiController]
	[Route("Controller")]
	public partial class ActionLogController : GenericController<Contract, Model>
	{
		[HttpGet("/api/[controller]/MaxPageSize")]
		public Task<int> GetMaxPageAsync()
		{
			return GetMaxPageAsync();
		}
		[HttpGet("/api/[controller]/Count")]
		public Task<int> GetCountAsync()
		{
			return CountModelsAsync();
		}
		[HttpGet("/api/[controller]/CountBy/{predicate}")]
		public Task<int> GetCountByAsync(string predicate)
		{
			return CountModelsByAsync(predicate);
		}
		[HttpGet("/api/[controller]/GetById/{id}")]
		public Task<Model> GetByIdAsync(int id)
		{
			return GetModelByIdAsync(id);
		}
		[HttpGet("/api/[controller]/GetPageList/{index}/{size}")]
		public Task<IEnumerable<Model>> GetPageListAsync(int index, int size)
		{
			return GetModelPageListAsync(index, size);
		}
		[HttpGet("/api/[controller]/GetAll")]
		public Task<IEnumerable<Model>> GetAllAsync()
		{
			return GetAllModelsAsync();
		}
		[HttpGet("/api/[controller]/QueryPageList/{predicate}/{index}/{size}")]
		public Task<IEnumerable<Model>> QueryPageListAsync(string predicate, int index, int size)
		{
			return QueryModelPageListAsync(predicate, index, size);
		}
		[HttpGet("/api/[controller]/QueryAll/{predicate}")]
		public Task<IEnumerable<Model>> QueryAllAsync(string predicate)
		{
			return QueryAllModelsAsync(predicate);
		}
		[HttpGet("/api/[controller]/Create")]
		public Task<Model> CreateAsync()
		{
			return CreateModelAsync();
		}
		[HttpPost("/api/[controller]")]
		public Task<Model> PostAsync(Model model)
		{
			return InsertModelAsync(model);
		}
		[HttpPut("/api/[controller]")]
		public Task<Model> PutAsync(Model model)
		{
			return UpdateModelAsync(model);
		}
		[HttpDelete("/api/[controller]/{id}")]
		public Task DeleteAsync(int id)
		{
			return DeleteModelAsync(id);
		}
		[HttpPost("/api/[controller]/CallAction")]
		public Task CallActionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeActionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
		[HttpPost("/api/[controller]/CallFunction")]
		public Task<Transfer.InvokeTypes.InvokeReturnValue> CallFunctionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeFunctionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
	}
}
namespace QnSContactManager.WebApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Contract = Contracts.Persistence.Account.IIdentity;
	using Model = Transfer.Persistence.Account.Identity;
	[ApiController]
	[Route("Controller")]
	public partial class IdentityController : GenericController<Contract, Model>
	{
		[HttpGet("/api/[controller]/MaxPageSize")]
		public Task<int> GetMaxPageAsync()
		{
			return GetMaxPageAsync();
		}
		[HttpGet("/api/[controller]/Count")]
		public Task<int> GetCountAsync()
		{
			return CountModelsAsync();
		}
		[HttpGet("/api/[controller]/CountBy/{predicate}")]
		public Task<int> GetCountByAsync(string predicate)
		{
			return CountModelsByAsync(predicate);
		}
		[HttpGet("/api/[controller]/GetById/{id}")]
		public Task<Model> GetByIdAsync(int id)
		{
			return GetModelByIdAsync(id);
		}
		[HttpGet("/api/[controller]/GetPageList/{index}/{size}")]
		public Task<IEnumerable<Model>> GetPageListAsync(int index, int size)
		{
			return GetModelPageListAsync(index, size);
		}
		[HttpGet("/api/[controller]/GetAll")]
		public Task<IEnumerable<Model>> GetAllAsync()
		{
			return GetAllModelsAsync();
		}
		[HttpGet("/api/[controller]/QueryPageList/{predicate}/{index}/{size}")]
		public Task<IEnumerable<Model>> QueryPageListAsync(string predicate, int index, int size)
		{
			return QueryModelPageListAsync(predicate, index, size);
		}
		[HttpGet("/api/[controller]/QueryAll/{predicate}")]
		public Task<IEnumerable<Model>> QueryAllAsync(string predicate)
		{
			return QueryAllModelsAsync(predicate);
		}
		[HttpGet("/api/[controller]/Create")]
		public Task<Model> CreateAsync()
		{
			return CreateModelAsync();
		}
		[HttpPost("/api/[controller]")]
		public Task<Model> PostAsync(Model model)
		{
			return InsertModelAsync(model);
		}
		[HttpPut("/api/[controller]")]
		public Task<Model> PutAsync(Model model)
		{
			return UpdateModelAsync(model);
		}
		[HttpDelete("/api/[controller]/{id}")]
		public Task DeleteAsync(int id)
		{
			return DeleteModelAsync(id);
		}
		[HttpPost("/api/[controller]/CallAction")]
		public Task CallActionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeActionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
		[HttpPost("/api/[controller]/CallFunction")]
		public Task<Transfer.InvokeTypes.InvokeReturnValue> CallFunctionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeFunctionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
	}
}
namespace QnSContactManager.WebApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Contract = Contracts.Persistence.Account.IIdentityXRole;
	using Model = Transfer.Persistence.Account.IdentityXRole;
	[ApiController]
	[Route("Controller")]
	public partial class IdentityXRoleController : GenericController<Contract, Model>
	{
		[HttpGet("/api/[controller]/MaxPageSize")]
		public Task<int> GetMaxPageAsync()
		{
			return GetMaxPageAsync();
		}
		[HttpGet("/api/[controller]/Count")]
		public Task<int> GetCountAsync()
		{
			return CountModelsAsync();
		}
		[HttpGet("/api/[controller]/CountBy/{predicate}")]
		public Task<int> GetCountByAsync(string predicate)
		{
			return CountModelsByAsync(predicate);
		}
		[HttpGet("/api/[controller]/GetById/{id}")]
		public Task<Model> GetByIdAsync(int id)
		{
			return GetModelByIdAsync(id);
		}
		[HttpGet("/api/[controller]/GetPageList/{index}/{size}")]
		public Task<IEnumerable<Model>> GetPageListAsync(int index, int size)
		{
			return GetModelPageListAsync(index, size);
		}
		[HttpGet("/api/[controller]/GetAll")]
		public Task<IEnumerable<Model>> GetAllAsync()
		{
			return GetAllModelsAsync();
		}
		[HttpGet("/api/[controller]/QueryPageList/{predicate}/{index}/{size}")]
		public Task<IEnumerable<Model>> QueryPageListAsync(string predicate, int index, int size)
		{
			return QueryModelPageListAsync(predicate, index, size);
		}
		[HttpGet("/api/[controller]/QueryAll/{predicate}")]
		public Task<IEnumerable<Model>> QueryAllAsync(string predicate)
		{
			return QueryAllModelsAsync(predicate);
		}
		[HttpGet("/api/[controller]/Create")]
		public Task<Model> CreateAsync()
		{
			return CreateModelAsync();
		}
		[HttpPost("/api/[controller]")]
		public Task<Model> PostAsync(Model model)
		{
			return InsertModelAsync(model);
		}
		[HttpPut("/api/[controller]")]
		public Task<Model> PutAsync(Model model)
		{
			return UpdateModelAsync(model);
		}
		[HttpDelete("/api/[controller]/{id}")]
		public Task DeleteAsync(int id)
		{
			return DeleteModelAsync(id);
		}
		[HttpPost("/api/[controller]/CallAction")]
		public Task CallActionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeActionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
		[HttpPost("/api/[controller]/CallFunction")]
		public Task<Transfer.InvokeTypes.InvokeReturnValue> CallFunctionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeFunctionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
	}
}
namespace QnSContactManager.WebApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Contract = Contracts.Persistence.Account.ILoginSession;
	using Model = Transfer.Persistence.Account.LoginSession;
	[ApiController]
	[Route("Controller")]
	public partial class LoginSessionController : GenericController<Contract, Model>
	{
		[HttpGet("/api/[controller]/MaxPageSize")]
		public Task<int> GetMaxPageAsync()
		{
			return GetMaxPageAsync();
		}
		[HttpGet("/api/[controller]/Count")]
		public Task<int> GetCountAsync()
		{
			return CountModelsAsync();
		}
		[HttpGet("/api/[controller]/CountBy/{predicate}")]
		public Task<int> GetCountByAsync(string predicate)
		{
			return CountModelsByAsync(predicate);
		}
		[HttpGet("/api/[controller]/GetById/{id}")]
		public Task<Model> GetByIdAsync(int id)
		{
			return GetModelByIdAsync(id);
		}
		[HttpGet("/api/[controller]/GetPageList/{index}/{size}")]
		public Task<IEnumerable<Model>> GetPageListAsync(int index, int size)
		{
			return GetModelPageListAsync(index, size);
		}
		[HttpGet("/api/[controller]/GetAll")]
		public Task<IEnumerable<Model>> GetAllAsync()
		{
			return GetAllModelsAsync();
		}
		[HttpGet("/api/[controller]/QueryPageList/{predicate}/{index}/{size}")]
		public Task<IEnumerable<Model>> QueryPageListAsync(string predicate, int index, int size)
		{
			return QueryModelPageListAsync(predicate, index, size);
		}
		[HttpGet("/api/[controller]/QueryAll/{predicate}")]
		public Task<IEnumerable<Model>> QueryAllAsync(string predicate)
		{
			return QueryAllModelsAsync(predicate);
		}
		[HttpGet("/api/[controller]/Create")]
		public Task<Model> CreateAsync()
		{
			return CreateModelAsync();
		}
		[HttpPost("/api/[controller]")]
		public Task<Model> PostAsync(Model model)
		{
			return InsertModelAsync(model);
		}
		[HttpPut("/api/[controller]")]
		public Task<Model> PutAsync(Model model)
		{
			return UpdateModelAsync(model);
		}
		[HttpDelete("/api/[controller]/{id}")]
		public Task DeleteAsync(int id)
		{
			return DeleteModelAsync(id);
		}
		[HttpPost("/api/[controller]/CallAction")]
		public Task CallActionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeActionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
		[HttpPost("/api/[controller]/CallFunction")]
		public Task<Transfer.InvokeTypes.InvokeReturnValue> CallFunctionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeFunctionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
	}
}
namespace QnSContactManager.WebApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Contract = Contracts.Persistence.Account.IRole;
	using Model = Transfer.Persistence.Account.Role;
	[ApiController]
	[Route("Controller")]
	public partial class RoleController : GenericController<Contract, Model>
	{
		[HttpGet("/api/[controller]/MaxPageSize")]
		public Task<int> GetMaxPageAsync()
		{
			return GetMaxPageAsync();
		}
		[HttpGet("/api/[controller]/Count")]
		public Task<int> GetCountAsync()
		{
			return CountModelsAsync();
		}
		[HttpGet("/api/[controller]/CountBy/{predicate}")]
		public Task<int> GetCountByAsync(string predicate)
		{
			return CountModelsByAsync(predicate);
		}
		[HttpGet("/api/[controller]/GetById/{id}")]
		public Task<Model> GetByIdAsync(int id)
		{
			return GetModelByIdAsync(id);
		}
		[HttpGet("/api/[controller]/GetPageList/{index}/{size}")]
		public Task<IEnumerable<Model>> GetPageListAsync(int index, int size)
		{
			return GetModelPageListAsync(index, size);
		}
		[HttpGet("/api/[controller]/GetAll")]
		public Task<IEnumerable<Model>> GetAllAsync()
		{
			return GetAllModelsAsync();
		}
		[HttpGet("/api/[controller]/QueryPageList/{predicate}/{index}/{size}")]
		public Task<IEnumerable<Model>> QueryPageListAsync(string predicate, int index, int size)
		{
			return QueryModelPageListAsync(predicate, index, size);
		}
		[HttpGet("/api/[controller]/QueryAll/{predicate}")]
		public Task<IEnumerable<Model>> QueryAllAsync(string predicate)
		{
			return QueryAllModelsAsync(predicate);
		}
		[HttpGet("/api/[controller]/Create")]
		public Task<Model> CreateAsync()
		{
			return CreateModelAsync();
		}
		[HttpPost("/api/[controller]")]
		public Task<Model> PostAsync(Model model)
		{
			return InsertModelAsync(model);
		}
		[HttpPut("/api/[controller]")]
		public Task<Model> PutAsync(Model model)
		{
			return UpdateModelAsync(model);
		}
		[HttpDelete("/api/[controller]/{id}")]
		public Task DeleteAsync(int id)
		{
			return DeleteModelAsync(id);
		}
		[HttpPost("/api/[controller]/CallAction")]
		public Task CallActionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeActionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
		[HttpPost("/api/[controller]/CallFunction")]
		public Task<Transfer.InvokeTypes.InvokeReturnValue> CallFunctionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeFunctionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
	}
}
namespace QnSContactManager.WebApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Contract = Contracts.Business.Account.IAppAccess;
	using Model = Transfer.Business.Account.AppAccess;
	[ApiController]
	[Route("Controller")]
	public partial class AppAccessController : GenericController<Contract, Model>
	{
		[HttpGet("/api/[controller]/MaxPageSize")]
		public Task<int> GetMaxPageAsync()
		{
			return GetMaxPageAsync();
		}
		[HttpGet("/api/[controller]/Count")]
		public Task<int> GetCountAsync()
		{
			return CountModelsAsync();
		}
		[HttpGet("/api/[controller]/CountBy/{predicate}")]
		public Task<int> GetCountByAsync(string predicate)
		{
			return CountModelsByAsync(predicate);
		}
		[HttpGet("/api/[controller]/GetById/{id}")]
		public Task<Model> GetByIdAsync(int id)
		{
			return GetModelByIdAsync(id);
		}
		[HttpGet("/api/[controller]/GetPageList/{index}/{size}")]
		public Task<IEnumerable<Model>> GetPageListAsync(int index, int size)
		{
			return GetModelPageListAsync(index, size);
		}
		[HttpGet("/api/[controller]/GetAll")]
		public Task<IEnumerable<Model>> GetAllAsync()
		{
			return GetAllModelsAsync();
		}
		[HttpGet("/api/[controller]/QueryPageList/{predicate}/{index}/{size}")]
		public Task<IEnumerable<Model>> QueryPageListAsync(string predicate, int index, int size)
		{
			return QueryModelPageListAsync(predicate, index, size);
		}
		[HttpGet("/api/[controller]/QueryAll/{predicate}")]
		public Task<IEnumerable<Model>> QueryAllAsync(string predicate)
		{
			return QueryAllModelsAsync(predicate);
		}
		[HttpGet("/api/[controller]/Create")]
		public Task<Model> CreateAsync()
		{
			return CreateModelAsync();
		}
		[HttpPost("/api/[controller]")]
		public Task<Model> PostAsync(Model model)
		{
			return InsertModelAsync(model);
		}
		[HttpPut("/api/[controller]")]
		public Task<Model> PutAsync(Model model)
		{
			return UpdateModelAsync(model);
		}
		[HttpDelete("/api/[controller]/{id}")]
		public Task DeleteAsync(int id)
		{
			return DeleteModelAsync(id);
		}
		[HttpPost("/api/[controller]/CallAction")]
		public Task CallActionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeActionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
		[HttpPost("/api/[controller]/CallFunction")]
		public Task<Transfer.InvokeTypes.InvokeReturnValue> CallFunctionAsync(Transfer.InvokeTypes.InvokeParam invokeParam)
		{
			return InvokeFunctionAsync(invokeParam.MethodName, invokeParam.GetParameters());
		}
	}
}
