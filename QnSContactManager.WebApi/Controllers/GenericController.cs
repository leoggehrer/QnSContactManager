//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBase.Extensions;
using QnSContactManager.Transfer.InvokeTypes;

namespace QnSContactManager.WebApi.Controllers
{
    public abstract class GenericController<I, M> : ApiControllerBase
        where I : Contracts.IIdentifiable
        where M : Transfer.IdentityModel, I, Contracts.ICopyable<I>, new()
    {
        protected Contracts.Client.IControllerAccess<I> CreateController()
        {
            var result = Logic.Factory.Create<I>();
            string authHeader = HttpContext.Request.Headers["Authorization"];
            string sessionToken = GetSessionToken(authHeader);

            if (sessionToken.HasContent())
            {
                result.SessionToken = sessionToken;
            }
            return result;
        }
        protected M ToModel(I entity)
        {
            var result = new M();

            result.CopyProperties(entity);
            return result;
        }

        protected async Task<int> CountModelsAsync()
        {
            using var ctrl = CreateController();

            return await ctrl.CountAsync().ConfigureAwait(false);
        }
        protected async Task<int> CountModelsByAsync(string predicate)
        {
            using var ctrl = CreateController();

            return await ctrl.CountByAsync(predicate).ConfigureAwait(false);
        }

        protected async Task<M> GetModelByIdAsync(int id)
        {
            using var ctrl = CreateController();

            var entity = (await ctrl.GetByIdAsync(id).ConfigureAwait(false));
            return ToModel(entity);
        }
        protected async Task<IEnumerable<M>> GetModelPageListAsync(int index, int size)
        {
            using var ctrl = CreateController();

            return (await ctrl.GetPageListAsync(index, size).ConfigureAwait(false)).ToList().Select(i => ToModel(i));
        }
        protected async Task<IEnumerable<M>> GetAllModelsAsync()
        {
            using var ctrl = CreateController();

            return (await ctrl.GetAllAsync().ConfigureAwait(false)).ToList().Select(i => ToModel(i));
        }

        protected async Task<IEnumerable<M>> QueryModelPageListAsync(string predicate, int index, int size)
        {
            using var ctrl = CreateController();

            return (await ctrl.QueryPageListAsync(predicate, index, size).ConfigureAwait(false)).ToList().Select(i => ToModel(i));
        }
        protected async Task<IEnumerable<M>> QueryAllModelsAsync(string predicate)
        {
            using var ctrl = CreateController();

            return (await ctrl.QueryAllAsync(predicate).ConfigureAwait(false)).ToList().Select(i => ToModel(i));
        }

        protected async Task<M> CreateModelAsync()
        {
            using var ctrl = CreateController();

            var entity = await ctrl.CreateAsync().ConfigureAwait(false);
            return ToModel(entity);
        }
        protected async Task<M> InsertModelAsync(M model)
        {
            using var ctrl = CreateController();

            var entity = await ctrl.InsertAsync(model).ConfigureAwait(false);

            await ctrl.SaveChangesAsync().ConfigureAwait(false);
            return ToModel(entity);
        }
        protected async Task<M> UpdateModelAsync(M model)
        {
            using var ctrl = CreateController();

            var entity = await ctrl.UpdateAsync(model).ConfigureAwait(false);

            await ctrl.SaveChangesAsync().ConfigureAwait(false);
            return ToModel(entity);
        }
        protected async Task DeleteModelAsync(int id)
        {
            using var ctrl = CreateController();

            await ctrl.DeleteAsync(id).ConfigureAwait(false);
            await ctrl.SaveChangesAsync().ConfigureAwait(false);
        }


        protected Task InvokeActionAsync(string name, string[] parameters)
        {
            name.CheckArgument(nameof(name));
            parameters.CheckArgument(nameof(parameters));

            using var ctrl = CreateController();

            return ctrl.InvokeActionAsync(name, parameters);
        }
        protected async Task<InvokeReturnValue> InvokeFunctionAsync(string name, string[] parameters)
        {
            name.CheckArgument(nameof(name));
            parameters.CheckArgument(nameof(parameters));
            static Type GetInterfaceType(Type t)
            {
                if (t != null && t.IsInterface)
                {
                    return t;
                }
                else if (t != null && t.GetInterfaces().Any())
                {
                    return t.GetInterfaces().Last();
                }
                return null;
            }

            using var ctrl = CreateController();
            var result = new InvokeReturnValue();
            var retVal = await ctrl.InvokeFunctionAsync(name, parameters).ConfigureAwait(false);

            if (retVal != null)
            {
                Type serializeType;

                result.IsArray = retVal.GetType().IsArray;
                if (result.IsArray == false)
                {
                    serializeType = retVal.GetType();
                }
                else
                {
                    serializeType = retVal.GetType().GetElementType();
                }

                if (serializeType.FullName.StartsWith("System.") == false)
                {
                    serializeType = GetInterfaceType(serializeType);
                }

                if (serializeType != null)
                {
                    result.Type = serializeType.FullName;
                    result.JsonData = System.Text.Json.JsonSerializer.Serialize(retVal);
                }
            }
            return result;
        }
    }
}
//MdEnd
