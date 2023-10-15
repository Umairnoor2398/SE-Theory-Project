using Microsoft.AspNetCore.Mvc;
using FreelancerCLone.ViewModels;
using FreelancerCLone.Utilities;

namespace FreelancerCLone.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserViewModel user) {
            UserUtility.Instance.AddUser(user);
            return RedirectToAction("Index");
        }
    }
}
