using FreelancerCLone.Models;
using FreelancerCLone.Services;
using FreelancerCLone.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FreelancerCLone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Feedback()
        {
            try
            {
                ViewBag.Categories = LookupUtility.Instance.getSelectList(Constants.LookupCategory.FEEDBACK_CATEGORY);
            }
            catch (Exception ex)
            {
                ViewBag.Categories = new List<SelectListItem>();
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View();

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