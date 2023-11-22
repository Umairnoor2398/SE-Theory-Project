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
        public IActionResult Index()
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();
            try
            {
                projects = ProjectUtility.Instance.GetProjects();
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
        public IActionResult BidRatePost(ProjectBid model)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();

                var bid = db.ProjectBids.Find(model.Id);
                bid.Rating = model.Rating;
                bid.IsReviewed = true;
                bid.UpdatedOn = DateTime.Now;
                db.ProjectBids.Update(bid);
                db.SaveChanges();
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
                FreelancerDbContext db = new FreelancerDbContext();
                project = db.Projects.Find(Id);
                ViewBag.ApprovedId = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
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
        public IActionResult BidProjectPost(ProjectBid model)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();
                model.IsActive = true;
                model.AddedOn = DateTime.Now;
                model.UpdatedOn = DateTime.Now;
                model.IsReviewed = false;
                model.IsCompleted = false;
                model.Status = db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id;
                model.Rating = 0;
                model.UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
                db.ProjectBids.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return RedirectToAction("Details", new { Id = model.ProjectId });
        }

        public IActionResult ApproveBid(int BidId, int ProjectId)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();

                var bid = db.ProjectBids.Find(BidId);
                bid.Status = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
                db.ProjectBids.Update(bid);
                db.SaveChanges();
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
                FreelancerDbContext db = new FreelancerDbContext();
                var bid = db.ProjectBids.Find(Id);
                bid.IsActive = false;
                db.ProjectBids.Update(bid);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("AssignedProjects");
        }


        public IActionResult ChangeProjectCompleteness(int Id)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();
                var bid = db.ProjectBids.Find(Id);
                bid.IsCompleted = !bid.IsCompleted;
                db.ProjectBids.Update(bid);
                db.SaveChanges();
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
                    FreelancerDbContext db = new FreelancerDbContext();
                    var p = db.Projects.Find(Id);
                    ProjectViewModel viewModel = new ProjectViewModel();
                    viewModel.Id = p.Id;
                    viewModel.Title = p.Title;
                    viewModel.Description = p.Description;
                    viewModel.Deadline = p.Deadline;
                    viewModel.IsActive = p.IsActive;
                    viewModel.Status = p.Status;
                    viewModel.TechnologyRequired = p.TechnologyRequired;
                    viewModel.Budget = p.Budget;
                    viewModel.ProjectBids = p.ProjectBids;
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
                    // Implement the Edit Functionailty
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
                FreelancerDbContext db = new FreelancerDbContext();
                var project = db.Projects.Find(Id);
                project.IsActive = false;
                db.Projects.Update(project);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("MyProjects");
        }

    }
}
