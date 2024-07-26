using Microsoft.AspNetCore.Mvc;
using Omega_Store.Models;
using Store.Business;
using Store.Model.ViewModel;
using System.Diagnostics;

namespace Omega_Store.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly StoreBusiness _storeBusiness;

        public BlogController(ILogger<BlogController> logger, StoreBusiness storeBusiness)
        {
            _logger = logger;
            _storeBusiness = storeBusiness;
        }
        [Route("Blog")]
        public IActionResult Index()
        {
            return View();
        }
    }
}