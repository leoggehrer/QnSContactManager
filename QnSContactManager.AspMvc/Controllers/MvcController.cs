//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using QnSContactManager.AspMvc.Modules.Session;

namespace QnSContactManager.AspMvc.Controllers
{
	public abstract partial class MvcController : Controller
	{
		protected IFactoryWrapper Factory { get; private set; }
		static MvcController()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();

		protected MvcController(IFactoryWrapper factoryWrapper)
		{
			Constructing();
			Factory = factoryWrapper;
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();

		#region SessionWrapper
		public bool IsSessionAvailable => HttpContext?.Session != null;
		private ISessionWrapper sessionWrapper = null;
		internal ISessionWrapper SessionWrapper => sessionWrapper ?? (sessionWrapper = new SessionWrapper(HttpContext.Session));
		#endregion

		protected static M ConvertTo<M, I>(I contract) 
			where M : Models.ModelObject, Contracts.ICopyable<I>, new()
		{
			contract.CheckArgument(nameof(contract));

			M result = new M();

			result.CopyProperties(contract);
			return result;
		}

		protected string ControllerName => GetType().Name.Replace("Controller", string.Empty);

		#region Error-helpers
		protected string GetModelStateError()
		{
			string[] errors = GetModelStateErrors();

			return string.Join($"{Environment.NewLine}", errors);
		}
		protected string[] GetModelStateErrors()
		{
			List<string> list = new List<string>();
			var errorLists = ModelState.Where(x => x.Value.Errors.Count > 0)
									   .Select(x => new { x.Key, x.Value.Errors });

			foreach (var errorList in errorLists)
			{
				foreach (var error in errorList.Errors)
				{
					list.Add($"{errorList.Key}: {error.ErrorMessage}");
				}
			}
			return list.ToArray();
		}

		protected static string GetExceptionError(Exception source)
		{
			source.CheckArgument(nameof(source));

			string tab = string.Empty;
			string errMsg = source.Message;
			Exception innerException = source.InnerException;

			while (innerException != null)
			{
				tab += "\t";
				errMsg = $"{errMsg}{Environment.NewLine}{tab}{innerException.Message}";
				innerException = innerException.InnerException;
			}
			return errMsg;
		}
		#endregion
	}
}
//MdEnd
