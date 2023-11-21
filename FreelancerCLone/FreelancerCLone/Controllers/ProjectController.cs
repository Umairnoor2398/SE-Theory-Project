using Microsoft.AspNetCore.Mvc;
using FreelancerCLone.ViewModels;
using FreelancerCLone.Utilities;
using FreelancerCLone.DbModels;
using Microsoft.AspNetCore.Authorization;

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
            var pro = ProjectUtility.Instance.GetProjects();
            return View(pro);
        }
        [Authorize]
        public IActionResult AssignedProjects()
        {
            int UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
            var userBids = ProjectUtility.Instance.GetUserBids(UserId);
            return View(userBids);
        }

        [Authorize]
        public IActionResult MyProjects()
        {
            int UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
            var userBids = ProjectUtility.Instance.GetUserAddedProjects(UserId);
            return View(userBids);
        }

        public IActionResult UserBidRate(int Id)
        {
            ViewBag.BidId = Id;
            return PartialView("UserBidRatePartialView");
        }

        [HttpPost]
        public IActionResult BidRatePost(ProjectBid model)
        {
            FreelancerDbContext db = new FreelancerDbContext();

            var bid = db.ProjectBids.Find(model.Id);
            bid.Rating = model.Rating;
            bid.IsReviewed = true;
            bid.UpdatedOn = DateTime.Now;
            db.ProjectBids.Update(bid);
            db.SaveChanges();
            return RedirectToAction("MyProjects");
        }

        public IActionResult Details(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var project = db.Projects.Find(Id);
            ViewBag.ApprovedId = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
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

            return RedirectToAction("Details", new { Id = model.ProjectId });
        }

        public IActionResult ApproveBid(int BidId, int ProjectId)
        {
            FreelancerDbContext db = new FreelancerDbContext();

            var bid = db.ProjectBids.Find(BidId);
            bid.Status = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
            db.ProjectBids.Update(bid);
            db.SaveChanges();

            return RedirectToAction("Details", new { Id = ProjectId });
        }

        public IActionResult DeleteBid(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var bid = db.ProjectBids.Find(Id);
            bid.IsActive = false;
            db.ProjectBids.Update(bid);
            db.SaveChanges();
            return RedirectToAction("AssignedProjects");
        }


        public IActionResult ChangeProjectCompleteness(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var bid = db.ProjectBids.Find(Id);
            bid.IsCompleted = !bid.IsCompleted;
            db.ProjectBids.Update(bid);
            db.SaveChanges();
            return RedirectToAction("AssignedProjects");
        }

        [Authorize]
        public IActionResult Create(int Id = 0)
        {
            ViewBag.Id = Id;
            if (Id != 0)
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
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProjectViewModel project)
        {
            if (project.Id == 0)
            {
                await ProjectUtility.Instance.AddProject(project, User.Identity.Name, _webHostEnvironment);
            }
            else
            {
                // Implement the Edit Functionailty
            }
            return RedirectToAction("MyProjects");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var project = db.Projects.Find(Id);
            project.IsActive = false;
            db.Projects.Update(project);
            db.SaveChanges();
            return RedirectToAction("MyProjects");
        }

    }
}
