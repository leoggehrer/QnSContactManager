//@QnSCodeCopy
//MdStart
using CommonBase.Extensions;
using CommonBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QnSContactManager.Logic.Controllers.Business
{
    abstract partial class BusinessControllerAdapter<I, E> : ControllerObject, Contracts.Client.IControllerAccess<I>
		where I : Contracts.IIdentifiable
		where E : Entities.IdentityObject, I, Contracts.ICopyable<I>, new()
	{
		static BusinessControllerAdapter()
		{
			ClassConstructing();
			ClassConstructed();
		}
		static partial void ClassConstructing();
		static partial void ClassConstructed();

		public virtual int MaxPageSize => throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");

		public BusinessControllerAdapter(DataContext.IContext context) : base(context)
		{
			Constructing();
			Constructed();
		}
		partial void Constructing();
		partial void Constructed();
		public BusinessControllerAdapter(ControllerObject controller) : base(controller)
		{
			Constructing();
			Constructed();
		}
		protected E ConvertTo(I contract)
		{
			contract.CheckArgument(nameof(contract));

			E result = new E();

			result.CopyProperties(contract);
			return result;
		}
		protected IEnumerable<E> ConvertTo(IEnumerable<I> contracts)
		{
			contracts.CheckArgument(nameof(contracts));

			List<E> result = new List<E>();

			foreach (var item in contracts)
			{
				result.Add(ConvertTo(item));
			}
			return result;
		}

		public virtual Task<int> CountAsync()
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}
		public virtual Task<int> CountByAsync(string predicate)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}
		#region Async-Methods
		public virtual Task<I> GetByIdAsync(int id)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}
		public virtual Task<IQueryable<I>> GetPageListAsync(int pageIndex, int pageSize)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}
		public virtual Task<IQueryable<I>> GetAllAsync()
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}

		public virtual Task<IQueryable<I>> QueryPageListAsync(string predicate, int pageIndex, int pageSize)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}
		public virtual Task<IQueryable<I>> QueryAllAsync(string predicate)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}


		public virtual Task<I> CreateAsync()
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}

		public virtual Task<I> InsertAsync(I entity)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}

		public virtual Task<I> UpdateAsync(I entity)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}

		public virtual Task DeleteAsync(int id)
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}

		public virtual Task SaveChangesAsync()
		{
			throw new NotSupportedException($"It is not supported: {MethodBase.GetCurrentMethod().GetOriginal()}!");
		}
		#endregion Async-Methods

		#region Invoke handler
		public virtual Task InvokeActionAsync(string name, params object[] parameters)
		{
			var helper = new InvokeHelper();

			return helper.InvokeActionAsync(this, name, parameters);
		}
		public virtual Task<object> InvokeFunctionAsync(string name, params object[] parameters)
		{
			var helper = new InvokeHelper();

			return helper.InvokeFunctionAsync(this, name, parameters);
		}
		#endregion Invoke handler
	}
}
//MdEnd
