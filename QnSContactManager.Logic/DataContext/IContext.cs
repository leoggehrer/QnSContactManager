//@QnSCodeCopy
//MdStart
using System;
using System.Linq;
using System.Threading.Tasks;
using QnSContactManager.Contracts;
using QnSContactManager.Logic.Entities;

namespace QnSContactManager.Logic.DataContext
{
    internal interface IContext : IDisposable
    {
        IQueryable<E> Set<I, E>()
            where I : IIdentifiable
            where E : IdentityObject, I;

        #region Async-Methods
        Task<int> CountAsync<I, E>()
            where I : IIdentifiable
            where E : IdentityObject, I;

        Task<int> CountByAsync<I, E>(string predicate)
            where I : IIdentifiable
            where E : IdentityObject, I;

        Task<E> CreateAsync<I, E>()
            where I : IIdentifiable
            where E : IdentityObject, ICopyable<I>, I, new();

        Task<E> InsertAsync<I, E>(E entity)
            where I : IIdentifiable
            where E : IdentityObject, ICopyable<I>, I, new();

        Task<E> UpdateAsync<I, E>(E entity)
            where I : IIdentifiable
            where E : IdentityObject, ICopyable<I>, I, new();

        Task<E> DeleteAsync<I, E>(int id)
            where I : IIdentifiable
            where E : IdentityObject, I;

        Task<int> SaveChangesAsync();
        #endregion Async-Methods
    }
}
//MdEnd
