//@QnSCustomizeCode
//MdStart
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QnSContactManager.AspMvc.Models;
using Contract = QnSContactManager.Contracts.Persistence.App.IContact;
using Model = QnSContactManager.AspMvc.Models.Persistence.App.Contact;

namespace QnSContactManager.AspMvc.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFactoryWrapper _factoryWrapper;
        public HomeController(ILogger<HomeController> logger, IFactoryWrapper factoryWrapper)
        {
            Constructing();
            _logger = logger;
            _factoryWrapper = factoryWrapper; 
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        private Model ConvertTo(Contract entity)
        {
            var result = new Model();

            result.CopyProperties(entity);
            return result;
        }
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            using var ctrl = _factoryWrapper.Create<Contract>();
            var entities = await ctrl.GetAllAsync();
            var models = entities.Select(e => ConvertTo(e));

            return View(models);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//MdEnd
