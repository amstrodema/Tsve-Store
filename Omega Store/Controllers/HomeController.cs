using App.Logger.Business;
using Microsoft.AspNetCore.Mvc;
using Omega_Store.Models;
using Omega_Store.Services;
using Store.Business;
using Store.Model.ViewModel;
using System.Diagnostics;

namespace Omega_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoreBusiness _storeBusiness;
        private readonly LoginValidator _loginValidator;
        private readonly LoggerBusiness _loggerBusiness;

        public HomeController(ILogger<HomeController> logger, StoreBusiness storeBusiness, LoginValidator loginValidator, LoggerBusiness loggerBusiness)
        {
            _logger = logger;
            _storeBusiness = storeBusiness;
            _loginValidator = loginValidator;
            _loggerBusiness = loggerBusiness;
        }

        public async Task<IActionResult> Index()
        {
            await _loggerBusiness.Traffic("Home", _loginValidator.GetUserID());
            var res = await _storeBusiness.GetVMForHome();
            return View(res);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("About")]
        public IActionResult About()
        {
            return View();
        }
        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("Faq")]
        public IActionResult Faq()
        {
            return View();
        }
        [Route("Help")]
        public IActionResult Help()
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