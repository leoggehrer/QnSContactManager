using QnSContactManager.Contracts.Persistence.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnSContactManager.Logic.Controllers.Persistence.App
{
    partial class ContactController
    {
        public override Task<int> CountAsync()
        {
            return ExecuteCountAsync();
        }
        public override Task<int> CountByAsync(string predicat)
        {
            return ExecuteCountByAsync(predicat);
        }
        public override Task<IQueryable<IContact>> GetPageListAsync(int pageIndex, int pageSize)
        {
            return ExecuteGetPageListAsync(pageIndex, pageSize);
        }
        public override Task<IQueryable<IContact>> GetAllAsync()
        {
            return ExecuteGetAllAsync();
        }
        public override Task<IQueryable<IContact>> QueryPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            return ExecuteQueryPageListAsync(predicate, pageIndex, pageSize);
        }
        public override Task<IQueryable<IContact>> QueryAllAsync(string predicate)
        {
            return ExecuteQueryAllAsync(predicate);
        }
    }
}
