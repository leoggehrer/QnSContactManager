//@QnSCodeCopy
//MdStart
using System.Collections.Generic;
using CommonBase.Extensions;
using QnSContactManager.Contracts.Business.Account;
using QnSContactManager.Contracts.Persistence.Account;
using QnSContactManager.Logic.Entities.Persistence.Account;

namespace QnSContactManager.Logic.Entities.Business.Account
{
    partial class AppAccess
    {
        internal Identity IdentityEntity { get; } = new Identity() { State = Contracts.Modules.Common.State.Active };
        partial void OnIdentityReading()
        {
            _identity = IdentityEntity;
        }
        internal List<Role> RoleEntities { get; } = new List<Role>();
        partial void OnRolesReading()
        {
            _roles = RoleEntities;
        }

        public override int Id { get => IdentityEntity.Id; set => IdentityEntity.Id = value; }
        public override byte[] Timestamp { get => IdentityEntity.Timestamp; set => IdentityEntity.Timestamp = value; }

        public void ClearRoles()
        {
            RoleEntities.Clear();
        }
        public IRole CreateRole()
        {
            return new Role();
        }
        public void AddRole(IRole role)
        {
            role.CheckArgument(nameof(role));
            var newItem = new Role();

            newItem.CopyProperties(role);
            RoleEntities.Add(newItem);
        }
        public void RemoveRole(IRole role)
        {
            role.CheckArgument(nameof(role));

            foreach (var item in RoleEntities)
            {
                if (item.Id != 0 && item.Id == role.Id)
                {
                    RoleEntities.Remove(item);
                }
                else if (item.Description != null && item.Description.Equals(role.Description, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    RoleEntities.Remove(item);
                }
            }
        }
        partial void BeforeCopyProperties(IAppAccess other, ref bool handled)
        {
            other.CheckArgument(nameof(other));
            other.Identity.CheckArgument(nameof(other.Identity));
            other.Roles.CheckArgument(nameof(other.Roles));

            handled = true;
            IdentityEntity.CopyProperties(other.Identity);
            RoleEntities.Clear();
            foreach (var item in other.Roles)
            {
                var newItem = new Role();

                newItem.CopyProperties(item);
                RoleEntities.Add(newItem);
            }
        }
    }
}
//MdEnd
