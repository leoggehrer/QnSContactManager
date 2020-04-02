//@QnSCodeCopy
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommonBase.Extensions;
using QnSContactManager.Contracts.Business.Account;
using QnSContactManager.Logic.Controllers.Persistence.Account;
using QnSContactManager.Logic.Entities.Business.Account;
using QnSContactManager.Logic.Entities.Persistence.Account;
using QnSContactManager.Logic.Modules.Account;
using QnSContactManager.Logic.Exceptions;

namespace QnSContactManager.Logic.Controllers.Business.Account
{
    partial class AppAccessController
    {
        private IdentityController identityController;
        private RoleController roleController;
        private IdentityXRoleController identityXroleController;

        partial void Constructed()
        {
            identityController = new IdentityController(this);
            roleController = new RoleController(this);
            identityXroleController = new IdentityXRoleController(this);
            ChangedSessionToken += AppAccessController_ChangedSessionToken;
        }

        private void AppAccessController_ChangedSessionToken(object sender, EventArgs e)
        {
            identityController.SessionToken = SessionToken;
            roleController.SessionToken = SessionToken;
            identityXroleController.SessionToken = SessionToken;
        }

        public override int MaxPageSize => identityController.MaxPageSize;

        public override Task<int> CountAsync()
        {
            CheckAuthorization(MethodBase.GetCurrentMethod());

            return identityController.CountAsync();
        }

        public override Task<IAppAccess> CreateAsync()
        {
            CheckAuthorization(MethodBase.GetCurrentMethod());

            return Task.Run<IAppAccess>(() => new AppAccess());
        }

        public override async Task<IAppAccess> GetByIdAsync(int id)
        {
            var result = default(AppAccess);
            var identity = await identityController.GetByIdAsync(id).ConfigureAwait(false);

            if (identity != null)
            {
                result = new AppAccess();
                result.Identity.CopyProperties(identity);
                foreach (var item in identityXroleController.ExecuteQuery(p => p.IdentityId == identity.Id).ToList())
                {
                    var role = await roleController.GetByIdAsync(item.RoleId).ConfigureAwait(false);

                    if (role != null)
                    {
                        var entity = new Role();

                        entity.CopyProperties(role);
                        result.RoleEntities.Add(entity);
                    }
                }
            }
            else
            {
                throw new LogicException(ErrorType.InvalidId);
            }
            return result;
        }
        public override Task<IQueryable<IAppAccess>> GetAllAsync()
        {
            return Task.Run<IQueryable<IAppAccess>>(async () =>
            {
                List<IAppAccess> result = new List<IAppAccess>();

                foreach (var item in (await identityController.GetAllAsync().ConfigureAwait(false)).ToList())
                {
                    result.Add(await GetByIdAsync(item.Id).ConfigureAwait(false));
                }
                return result.AsQueryable();
            });
        }
        public override Task<IQueryable<IAppAccess>> GetPageListAsync(int pageIndex, int pageSize)
        {
            return Task.Run<IQueryable<IAppAccess>>(async () =>
            {
                List<IAppAccess> result = new List<IAppAccess>();

                foreach (var item in (await identityController.GetPageListAsync(pageIndex, pageSize).ConfigureAwait(false)).ToList())
                {
                    result.Add(await GetByIdAsync(item.Id).ConfigureAwait(false));
                }
                return result.AsQueryable();
            });
        }
        public override Task<IQueryable<IAppAccess>> QueryPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            return Task.Run<IQueryable<IAppAccess>>(async () =>
            {
                List<IAppAccess> result = new List<IAppAccess>();

                foreach (var item in (await identityController.QueryPageListAsync(predicate, pageIndex, pageSize).ConfigureAwait(false)).ToList())
                {
                    result.Add(await GetByIdAsync(item.Id).ConfigureAwait(false));
                }
                return result.AsQueryable();
            });
        }

        public override async Task<IAppAccess> InsertAsync(IAppAccess entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.Identity.CheckArgument(nameof(entity.Identity));
            entity.Roles.CheckArgument(nameof(entity.Roles));

            var result = new AppAccess();

            result.IdentityEntity.CopyProperties(entity.Identity);
            result.IdentityEntity.PasswordHash = AccountManager.CalculateHash(result.IdentityEntity.Password);
            await identityController.InsertAsync(result.IdentityEntity).ConfigureAwait(false);
            foreach (var item in entity.Roles)
            {
                var role = new Role();
                var joinRole = new IdentityXRole();

                joinRole.Identity = result.IdentityEntity;
                if (item.Id == 0)
                {
                    item.Designation = RoleController.ClearRoleDesignation(item.Designation);

                    var qryItem = roleController.ExecuteQuery(e => e.Designation.Equals(item.Designation)).FirstOrDefault();

                    if (qryItem != null)
                    {
                        role.CopyProperties(qryItem);
                        joinRole.RoleId = role.Id;
                    }
                    else
                    {
                        role.CopyProperties(item);
                        await roleController.InsertAsync(role).ConfigureAwait(false);
                        joinRole.Role = role;
                    }
                }
                else
                {
                    var qryItem = await roleController.GetByIdAsync(item.Id).ConfigureAwait(false);

                    if (qryItem != null)
                    {
                        role.CopyProperties(qryItem);
                    }
                    joinRole.RoleId = role.Id;
                }
                await identityXroleController.InsertAsync(joinRole).ConfigureAwait(false);
                result.RoleEntities.Add(role);
            }
            return result;
        }
        public override async Task<IAppAccess> UpdateAsync(IAppAccess entity)
        {
            entity.CheckArgument(nameof(entity));
            entity.Identity.CheckArgument(nameof(entity.Identity));
            entity.Roles.CheckArgument(nameof(entity.Roles));

            //Delete all costs that are no longer included in the list.
            var identXRoles = identityXroleController.ExecuteQuery(e => e.IdentityId == entity.Identity.Id).ToList();
            foreach (var item in identXRoles)
            {
                var tmpItem = entity.Roles.SingleOrDefault(i => i.Id == item.RoleId);

                if (tmpItem == null)
                {
                    await identityXroleController.DeleteAsync(item.Id).ConfigureAwait(false);
                }
            }

            var result = new AppAccess();
            var identity = await identityController.UpdateAsync(entity.Identity).ConfigureAwait(false);

            foreach (var item in entity.Roles)
            {
                var role = new Role();
                var joinRole = new IdentityXRole();

                role.Id = item.Id;
                joinRole.IdentityId = identity.Id;
                if (item.Id == 0)
                {
                    item.Designation = RoleController.ClearRoleDesignation(role.Designation);
                    var qryItem = roleController.ExecuteQuery(e => e.Designation.Equals(item.Designation))
                                                .FirstOrDefault();

                    if (qryItem != null)
                    {
                        role.CopyProperties(qryItem);
                        joinRole.RoleId = role.Id;
                    }
                    else
                    {
                        role.CopyProperties(item);
                        await roleController.InsertAsync(role).ConfigureAwait(false);
                        joinRole.Role = role;
                    }
                }
                else
                {
                    var qryItem = await roleController.GetByIdAsync(role.Id).ConfigureAwait(false);

                    if (qryItem != null)
                    {
                        role.CopyProperties(qryItem);
                        joinRole.RoleId = role.Id;
                    }
                }
                var tmpItem = identXRoles.SingleOrDefault(e => e.IdentityId == joinRole.IdentityId && e.RoleId == joinRole.RoleId);

                if (tmpItem == null)
                {
                    await identityXroleController.InsertAsync(joinRole).ConfigureAwait(false);
                }
                result.RoleEntities.Add(role);
            }
            return result;
        }
        public override async Task DeleteAsync(int id)
        {
            //Delete all costs that are no longer included in the list.
            var identXRoles = identityXroleController.ExecuteQuery(e => e.IdentityId == id).ToList();

            foreach (var item in identXRoles)
            {
                await identityXroleController.DeleteAsync(item.Id).ConfigureAwait(false);
            }
            await identityController.DeleteAsync(id).ConfigureAwait(false);
        }
        public override Task SaveChangesAsync()
        {
            CheckAuthorization(MethodBase.GetCurrentMethod());

            return Context.SaveChangesAsync();
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            identityController.Dispose();
            roleController.Dispose();
            identityXroleController.Dispose();

            identityController = null;
            roleController = null;
            identityXroleController = null;
        }
    }
}
//MdENd
