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

        public IActionResult AssignedProjects()
        {
            int UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
            var userBids = ProjectUtility.Instance.GetUserBids(UserId);
            return View(userBids);
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProjectViewModel project)
        {
            await ProjectUtility.Instance.AddProject(project, User.Identity.Name, _webHostEnvironment);
            return View();
        }

    }
}
