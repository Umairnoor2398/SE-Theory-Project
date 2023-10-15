using Microsoft.AspNetCore.Mvc;
using FreelancerCLone.ViewModels;
using FreelancerCLone.Utilities;

namespace FreelancerCLone.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            var pro = ProjectUtility.Instance.GetProjects();
            return View(pro);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel project)
        {
            ProjectUtility.Instance.AddProject(project);
            return View();
        }

    }
}
