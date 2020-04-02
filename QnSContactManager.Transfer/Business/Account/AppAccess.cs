//@QnSCodeCopy
//MdStart
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommonBase.Extensions;
using QnSContactManager.Contracts.Business.Account;
using QnSContactManager.Contracts.Persistence.Account;
using QnSContactManager.Transfer.Persistence.Account;

namespace QnSContactManager.Transfer.Business.Account
{
    partial class AppAccess
    {
        [JsonPropertyName(nameof(Identity))]
        public Identity IdentityEntity { get; set; } = new Identity();
        [JsonPropertyName(nameof(Roles))]
        public List<Role> RoleEntities { get; set; } = new List<Role>();

        public override int Id { get => IdentityEntity.Id; set => IdentityEntity.Id = value; }
        public override byte[] Timestamp { get => IdentityEntity.Timestamp; set => IdentityEntity.Timestamp = value; }

        partial void OnIdentityReading()
        {
            _identity = IdentityEntity;
        }
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
                var role = new Role();

                role.CopyProperties(item);
                RoleEntities.Add(role);
            }
        }
    }
}
//MdEnd
