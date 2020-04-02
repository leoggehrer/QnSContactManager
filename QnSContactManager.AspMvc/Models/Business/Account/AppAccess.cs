//@QnSCodeCopy
//MdStart
using System.Collections.Generic;
using CommonBase.Extensions;
using QnSContactManager.AspMvc.Models.Persistence.Account;
using QnSContactManager.Contracts.Business.Account;
using QnSContactManager.Contracts.Persistence.Account;

namespace QnSContactManager.AspMvc.Models.Business.Account
{
    public partial class AppAccess
    {
        internal Identity IdentityEntity = new Identity();
        partial void OnIdentityReading()
        {
            _identity = IdentityEntity;
        }

        internal List<Role> RoleEntities = new List<Role>();
        partial void OnRolesReading()
        {
            _roles = RoleEntities;
        }

        public void ClearRoles()
        {
            RoleEntities.Clear();
        }
        public IRole CreateRole()
        {
            var result = new Role();

            return result;
        }
        public void AddRole(IRole entity)
        {
            entity.CheckArgument(nameof(entity));

            var item = new Role();

            item.CopyProperties(entity);
            RoleEntities.Add(item);
        }
        public void RemoveRole(IRole entity)
        {
            entity.CheckArgument(nameof(entity));

            foreach (var item in RoleEntities)
            {
                if (entity.Id != 0 && entity.Id == item.Id)
                {
                    RoleEntities.Remove(item);
                }
                else if (item.Description != null && item.Description.Equals(item.Description))
                {
                    RoleEntities.Remove(item);
                }
            }
        }

        partial void AfterCopyProperties(IAppAccess other)
        {
            IdentityEntity.CopyProperties(other.Identity);
            RoleEntities.Clear();

            foreach (var item in other.Roles)
            {
                var entity = new Role();

                entity.CopyProperties(item);
                RoleEntities.Add(entity);
            }
        }
    }
}
//MdEnd
