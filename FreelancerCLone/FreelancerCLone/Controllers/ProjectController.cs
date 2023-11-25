using Microsoft.AspNetCore.Mvc;
using FreelancerCLone.ViewModels;
using FreelancerCLone.Utilities;
using FreelancerCLone.DbModels;
using Microsoft.AspNetCore.Authorization;
using FreelancerCLone.Services;

namespace FreelancerCLone.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string query)
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();
            try
            {

                projects = ProjectUtility.Instance.GetProjects(User.Identity.Name);

                if (!string.IsNullOrWhiteSpace(query))
                {
                    string[] queryWords = query.Split(' ');
                    projects = projects.Where(item =>
                               queryWords.All(word => item.Title.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                               item.TechnologyRequired.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                               item.Description.Contains(word, StringComparison.OrdinalIgnoreCase))).ToList();
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View(projects);
        }
        [Authorize]
        public IActionResult AssignedProjects()
        {
            List<ProjectBid> userBids = new List<ProjectBid>();
            try
            {
                int UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
                userBids = ProjectUtility.Instance.GetUserBids(UserId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View(userBids);
        }

        [Authorize]
        public IActionResult MyProjects()
        {
            List<ProjectViewModel> userprojects = new List<ProjectViewModel>();
            try
            {
                int UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
                userprojects = ProjectUtility.Instance.GetUserAddedProjects(UserId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View(userprojects);
        }

        public IActionResult UserBidRate(int Id)
        {
            ViewBag.BidId = Id;
            return PartialView("UserBidRatePartialView");
        }

        [HttpPost]
        public async Task<IActionResult> BidRatePostAsync(ProjectBid model)
        {
            try
            {
                var bid = ProjectUtility.Instance.RateUserProjectBid(model);
                await MailSenderService.Instance.SendMailRateProject(bid);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("MyProjects");
        }



        public IActionResult Details(int Id)
        {
            Project project = new Project();
            try
            {
                project = ProjectUtility.Instance.GetProject(Id);

                ViewBag.ApprovedId = LookupUtility.Instance.getId("Accepted");
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View(project);
        }



        public IActionResult BidProject(int Id)
        {
            ViewBag.ProjectId = Id;
            return PartialView("ProjectBidPartialView");
        }

        [HttpPost]
        public async Task<IActionResult> BidProjectPostAsync(ProjectBid model)
        {
            try
            {
                model = ProjectUtility.Instance.AddProjectBid(model, User.Identity.Name);
                await MailSenderService.Instance.SendMailOnReceiveBidInProject(model);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return RedirectToAction("Details", new { Id = model.ProjectId });
        }



        public async Task<IActionResult> ApproveBidAsync(int BidId, int ProjectId)
        {
            try
            {
                var bid = ProjectUtility.Instance.ApproveUserProjectBid(BidId);
                await MailSenderService.Instance.SendMailOnApproveBidInProject(bid);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return RedirectToAction("Details", new { Id = ProjectId });
        }

        public IActionResult DeleteBid(int Id)
        {
            try
            {
                ProjectUtility.Instance.DeleteUserProjectBid(Id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("AssignedProjects");
        }



        public async Task<IActionResult> ChangeProjectCompletenessAsync(int Id)
        {
            try
            {
                var bid = ProjectUtility.Instance.UpdateProjectCompleteStatus(Id);
                await MailSenderService.Instance.SendMailOnProjectCompletenesssUpdate(bid);


            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("AssignedProjects");
        }



        [Authorize]
        public IActionResult Create(int Id = 0)
        {
            ViewBag.Id = Id;
            if (Id != 0)
            {
                try
                {
                    ProjectViewModel viewModel = ProjectUtility.Instance.GetProjectViewModel(Id);
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                }
            }
            return View();
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProjectViewModel project)
        {
            try
            {
                if (project.Id == 0)
                {
                    await ProjectUtility.Instance.AddProject(project, User.Identity.Name, _webHostEnvironment);
                }
                else
                {
                    await ProjectUtility.Instance.UpdateProject(project, _webHostEnvironment);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("MyProjects");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                ProjectUtility.Instance.DeleteProject(Id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("MyProjects");
        }


    }
}
