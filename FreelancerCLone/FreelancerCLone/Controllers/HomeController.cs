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

        // Constructor for HomeController, initializes the logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Displays the default home page
        public IActionResult Index()
        {
            return View();
        }

        // Displays the Feedback page with feedback category options
        public IActionResult Feedback()
        {
            try
            {
                // Populates ViewBag with feedback category options
                ViewBag.Categories = LookupUtility.Instance.getSelectList(Constants.LookupCategory.FEEDBACK_CATEGORY);
            }
            catch (Exception ex)
            {
                // Handles exceptions by providing an empty list and logging the error
                ViewBag.Categories = new List<SelectListItem>();
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View();
        }

        // Handles errors and displays the Error view 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
