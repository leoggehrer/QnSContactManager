//@QnSCodeCopy
//MdStart
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace QnSContactManager.Adapters.Controller
{
    partial class GenericControllerAdapter<TContract> : Contracts.Client.IAdapterAccess<TContract>
        where TContract : Contracts.IIdentifiable
    {
        static GenericControllerAdapter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public Contracts.Client.IControllerAccess<TContract> controller;

        public GenericControllerAdapter()
        {
            Constructing();
            controller = Logic.Factory.Create<TContract>();
            Constructed();
        }
        public GenericControllerAdapter(string sessionToken)
        {
            Constructing();
            controller = Logic.Factory.Create<TContract>(sessionToken);
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public string SessionToken { set => controller.SessionToken = value; }
        public int MaxPageSize => controller.MaxPageSize;

        #region Async-Methods
        public Task<int> CountAsync()
        {
            return controller.CountAsync();
        }
        public Task<int> CountByAsync(string predicate)
        {
            return controller.CountByAsync(predicate);
        }

        public Task<TContract> GetByIdAsync(int id)
        {
            return controller.GetByIdAsync(id);
        }
        public async Task<IEnumerable<TContract>> GetPageListAsync(int pageIndex, int pageSize)
        {
            return (await controller.GetPageListAsync(pageIndex, pageSize).ConfigureAwait(false)).ToArray();
        }
        public async Task<IEnumerable<TContract>> GetAllAsync()
        {
            return (await controller.GetAllAsync().ConfigureAwait(false)).ToArray();
        }

        public async Task<IEnumerable<TContract>> QueryPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            return (await controller.QueryPageListAsync(predicate, pageIndex, pageSize).ConfigureAwait(false)).ToArray();
        }
        public async Task<IEnumerable<TContract>> QueryAllAsync(string predicate)
        {
            return (await controller.QueryAllAsync(predicate).ConfigureAwait(false)).ToArray();
        }

        public Task<TContract> CreateAsync()
        {
            return controller.CreateAsync();
        }

        public async Task<TContract> InsertAsync(TContract entity)
        {
            var result = await controller.InsertAsync(entity).ConfigureAwait(false);

            await controller.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<TContract> UpdateAsync(TContract entity)
        {
            var result = await controller.UpdateAsync(entity).ConfigureAwait(false);

            await controller.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await controller.DeleteAsync(id).ConfigureAwait(false);
            await controller.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task InvokeActionAsync(string name, params object[] parameters)
        {
            return controller.InvokeActionAsync(name, parameters);
        }

        public async Task<TResult> InvokeFunctionAsync<TResult>(string name, params object[] parameters)
        {
            var result = default(TResult);
            var invokeResult = await controller.InvokeFunctionAsync(name, parameters).ConfigureAwait(false);

            if (invokeResult != null)
            {
                var json = JsonSerializer.Serialize(invokeResult);

                result = JsonSerializer.Deserialize<TResult>(json);
            }
            return result;
        }
        #endregion Async-Methods

        public void Dispose()
        {
            controller?.Dispose();
            controller = null;
        }
    }
}
//MdEnd
