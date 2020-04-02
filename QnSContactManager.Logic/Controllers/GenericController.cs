//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Reflection;
using CommonBase.Extensions;
using QnSContactManager.Contracts.Client;
using QnSContactManager.Logic.DataContext;
using QnSContactManager.Logic.Modules.Security;
using System.Linq.Expressions;
using QnSContactManager.Logic.Exceptions;
using CommonBase.Helpers;

namespace QnSContactManager.Logic.Controllers
{
    /// <inheritdoc cref="IControllerAccess{T}"/>
    /// <summary>
    /// This generic class implements the base properties and operations defined in the interface. 
    /// </summary>
    /// <typeparam name="E">The entity type of element in the controller.</typeparam>
    /// <typeparam name="I">The interface type which implements the entity.</typeparam>
    [Authorize]
    internal abstract partial class GenericController<I, E> : ControllerObject, IControllerAccess<I>
        where I : Contracts.IIdentifiable
        where E : Entities.IdentityObject, I, Contracts.ICopyable<I>, new()
    {
        static GenericController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public int MaxPageSize => 500;

        internal IQueryable<E> Set() => Context.Set<I, E>();

        protected GenericController(IContext context)
            : base(context)
        {
            Constructing();
            Constructed();
        }
        protected GenericController(ControllerObject controllerObject)
            : base(controllerObject)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

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
        #region Async-Methods
        public virtual Task<int> CountAsync()
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteCountAsync();
        }
        internal Task<int> ExecuteCountAsync()
        {
            return Context.CountAsync<I, E>();
        }
        public virtual Task<int> CountByAsync(string predicat)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteCountByAsync(predicat);
        }
        internal Task<int> ExecuteCountByAsync(string predicate)
        {
            return Context.CountByAsync<I, E>(predicate);
        }

        public virtual Task<I> GetByIdAsync(int id)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteGetByIdAsync(id);
        }
        internal virtual Task<I> ExecuteGetByIdAsync(int id)
        {
            return Task.Run<I>(() => Set().SingleOrDefault(i => i.Id == id));
        }

        public virtual Task<IQueryable<I>> GetPageListAsync(int pageIndex, int pageSize)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteGetPageListAsync(pageIndex, pageSize);
        }
        internal virtual Task<IQueryable<I>> ExecuteGetPageListAsync(int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            return Task.FromResult<IQueryable<I>>(Set().Skip(pageIndex * pageSize).Take(pageSize));
        }

        public virtual Task<IQueryable<I>> GetAllAsync()
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteGetAllAsync();
        }
        internal async virtual Task<IQueryable<I>> ExecuteGetAllAsync()
        {
            int idx = 0, qryCount = 0;
            List<I> result = new List<I>();

            do
            {
                var qry = await ExecuteGetPageListAsync(idx++, MaxPageSize).ConfigureAwait(false);

                qryCount = qry.Count();
                result.AddRange(qry);
            } while (qryCount == MaxPageSize);
            return result.AsQueryable();
        }

        public virtual Task<IQueryable<I>> QueryPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteQueryPageListAsync(predicate, pageIndex, pageSize);
        }
        internal virtual Task<IQueryable<I>> ExecuteQueryPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            return Task.FromResult<IQueryable<I>>(Set().AsQueryable()
                     .Where(predicate)
                     .Skip(pageIndex * pageSize)
                     .Take(pageSize));
        }

        public virtual Task<IQueryable<I>> QueryAllAsync(string predicate)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteQueryAllAsync(predicate);
        }
        internal virtual async Task<IQueryable<I>> ExecuteQueryAllAsync(string predicate)
        {
            int idx = 0, qryCount = 0;
            List<I> result = new List<I>();

            do
            {
                var qry = await ExecuteQueryPageListAsync(predicate, idx++, MaxPageSize).ConfigureAwait(false);

                qryCount = qry.Count();
                result.AddRange(qry);
            } while (qryCount == MaxPageSize);
            return result.AsQueryable();
        }

        internal virtual Task<IQueryable<I>> ExecuteGroupByAsync(string keySelector, string resultSelector)
        {
            return Task.FromResult<IQueryable<I>>(Set().GroupBy(keySelector, resultSelector) as IQueryable<I>);
        }
        public virtual Task<I> CreateAsync()
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteCreateAsync();
        }
        internal virtual Task<I> ExecuteCreateAsync()
        {
            return Task.Run<I>(() => new E());
        }

        protected virtual Task BeforeInsertingUpdateingAsync(E entiy)
        {
            return Task.FromResult(0);
        }
        protected virtual Task BeforeInsertingAsync(E entity)
        {
            return Task.FromResult(0);
        }

        public virtual Task<I> InsertAsync(I entity)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteInsertAsync(ConvertTo(entity));
        }
        internal virtual Task<I> InsertAsync(E entity)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());
            entity.CheckArgument(nameof(entity));

            return ExecuteInsertAsync(entity);
        }
        internal virtual async Task<I> ExecuteInsertAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));

            await BeforeInsertingUpdateingAsync(entity).ConfigureAwait(false);
            await BeforeInsertingAsync(entity).ConfigureAwait(false);
            var result = await Context.InsertAsync<I, E>(entity).ConfigureAwait(false);
            await AfterInsertedAsync(result).ConfigureAwait(false);
            await AfterInsertedUpdatedAsync(result).ConfigureAwait(false);
            return result;
        }
        protected virtual Task AfterInsertedAsync(E entity)
        {
            return Task.FromResult(0);
        }

        protected virtual Task BeforeUpdatingAsync(E entity)
        {
            return Task.FromResult(0);
        }
        public virtual Task<I> UpdateAsync(I entity)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());
            entity.CheckArgument(nameof(entity));

            var entityModel = Set().SingleOrDefault(i => i.Id == entity.Id);

            if (entityModel != null)
            {
                entityModel.CopyProperties(entity);
                return ExecuteUpdateAsync(entityModel);
            }
            else
            {
                throw new LogicException(ErrorType.InvalidId);
            }
        }
        internal virtual Task<I> UpdateAsync(E entity)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());
            entity.CheckArgument(nameof(entity));

            return ExecuteUpdateAsync(entity);
        }
        internal virtual async Task<I> ExecuteUpdateAsync(E entity)
        {
            entity.CheckArgument(nameof(entity));

            await BeforeInsertingUpdateingAsync(entity).ConfigureAwait(false);
            await BeforeUpdatingAsync(entity).ConfigureAwait(false);
            var result = await Context.UpdateAsync<I, E>(entity).ConfigureAwait(false);
            await AfterUpdatedAsync(result).ConfigureAwait(false);
            await AfterInsertedUpdatedAsync(result).ConfigureAwait(false);
            return result;
        }
        protected virtual Task AfterUpdatedAsync(E entity)
        {
            return Task.FromResult(0);
        }
        protected virtual Task AfterInsertedUpdatedAsync(E entiy)
        {
            return Task.FromResult(0);
        }

        protected virtual Task BeforeDeletingAsync(int id)
        {
            return Task.FromResult(0);
        }
        public Task DeleteAsync(int id)
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteDeleteAsync(id);
        }
        internal async Task ExecuteDeleteAsync(int id)
        {
            await BeforeDeletingAsync(id).ConfigureAwait(false);
            var item = await Context.DeleteAsync<I, E>(id).ConfigureAwait(false);

            if (item != null)
            {
                await AfterDeletedAsync(item).ConfigureAwait(false);
            }
        }
        protected virtual Task AfterDeletedAsync(E entity)
        {
            return Task.FromResult(0);
        }

        protected virtual Task BeforeSaveChangesAsync()
        {
            return Task.FromResult(0);
        }
        public Task SaveChangesAsync()
        {
            CheckAuthorization(GetType(), MethodBase.GetCurrentMethod());

            return ExecuteSaveChangesAsync();
        }
        internal async Task ExecuteSaveChangesAsync()
        {
            await BeforeSaveChangesAsync().ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
            await AfterSaveChangesAsync().ConfigureAwait(false);
        }
        protected virtual Task AfterSaveChangesAsync()
        {
            return Task.FromResult(0);
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

        #region Internal-Methods
        internal virtual E ExecuteQueryById(int id)
        {
            return Set().SingleOrDefault(i => i.Id == id);
        }
        internal virtual IQueryable<E> ExecuteQuery(Expression<Func<E, bool>> predicate)
        {
            return Set().Where(predicate);
        }
        internal virtual IQueryable<E> ExecuteQuery(string predicate, int pageIndex, int pageSize)
        {
            if (pageSize < 1 && pageSize > MaxPageSize)
                throw new LogicException(ErrorType.InvalidPageSize);

            return Set().Where(predicate)
                       .Skip(pageIndex * pageSize)
                       .Take(pageSize);
        }

        //internal virtual async Task<IEnumerable<I>> QueryAsyncFacade(Expression<Func<I, bool>> predicate)
        //{
        //    var newPredicate = ExpressionConverter.ConvertToObject<I, bool, E, bool>(predicate);

        //    return await QueryAsync(newPredicate).ToArrayAsync().ConfigureAwait(false);
        //}
        #endregion Internal-Methods
    }
}
//MdEnd
