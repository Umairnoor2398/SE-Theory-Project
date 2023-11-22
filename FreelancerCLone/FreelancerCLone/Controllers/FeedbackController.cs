using FreelancerCLone.DbModels;
using FreelancerCLone.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerCLone.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var feedbacks = db.Feedbacks.Where(x => x.Status == db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id).ToList();
            return View(feedbacks);
        }

        [HttpPost]
        public IActionResult Create(Feedback model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            model.AddedOn = DateTime.Now;
            model.Status = db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id;
            model.AddedBy = UserUtility.Instance.GetUserId(User.Identity.Name);
            db.Feedbacks.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            Feedback model = db.Feedbacks.Where(x => x.Id == Id).FirstOrDefault();
            return PartialView("FeedbackDetailsPartialView", model);
        }

        public IActionResult UpdateStatus(Feedback model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            Feedback feedback = db.Feedbacks.Where(x => x.Id == model.Id).FirstOrDefault();
            feedback.Status = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
            db.Feedbacks.Update(feedback);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
