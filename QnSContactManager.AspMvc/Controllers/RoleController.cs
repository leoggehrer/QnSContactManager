//@QnSCodeCopy
//MdStart
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model = QnSContactManager.AspMvc.Models.Persistence.Account.Role;
using Contract = QnSContactManager.Contracts.Persistence.Account.IRole;

namespace QnSContactManager.AspMvc.Controllers
{
    public partial class RoleController : AccessController
    {
        private readonly ILogger<IdentityController> _logger;
        public RoleController(ILogger<IdentityController> logger, IFactoryWrapper factoryWrapper)
            : base(factoryWrapper)
        {
            Constructing();
            _logger = logger;
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

            return View(entities.Select(e => ConvertTo<Model, Contract>(e))); 
        }
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            var entity = await ctrl.CreateAsync().ConfigureAwait(false);

            return View(ConvertTo<Model, Contract>(entity));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync(Model model)
        {
            if (ModelState.IsValid == false)
            {
                model.ActionError = GetModelStateError(); 
                return View(model);
            }
            try
            {
                using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);

                await ctrl.InsertAsync(model).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                model.ActionError = GetExceptionError(ex);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(int id)
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

            return View(ConvertTo<Model, Contract>(entity));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(int id, Model model)
        {
            if (ModelState.IsValid == false)
            {
                model.ActionError = GetModelStateError();
                return View(model);
            }
            try
            {
                using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);

                await ctrl.UpdateAsync(model).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                model.ActionError = GetExceptionError(ex);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int id, string error = null)
        {
            using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
            var model = ConvertTo<Model, Contract>(entity);

            model.ActionError = error;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int id, Model model)
        {
            if (ModelState.IsValid == false)
            {
                return RedirectToAction("Delete", new { id, error = GetModelStateError() });
            }
            try
            {
                using var ctrl = Factory.Create<Contract>(SessionWrapper.SessionToken);

                await ctrl.DeleteAsync(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Delete", new { id, error = GetExceptionError(ex) });
            }
            return RedirectToAction("Index");
        }
    }
}
//MdEnd
