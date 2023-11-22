using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerCLone.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();
                feedbacks = db.Feedbacks.Where(x => x.Status == db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View(feedbacks);
        }

        [HttpPost]
        public IActionResult Create(Feedback model)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();
                model.AddedOn = DateTime.Now;
                model.Status = db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id;
                model.AddedBy = UserUtility.Instance.GetUserId(User.Identity.Name);
                db.Feedbacks.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int Id)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();
                Feedback model = db.Feedbacks.Where(x => x.Id == Id).FirstOrDefault();
                return PartialView("FeedbackDetailsPartialView", model);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateStatus(Feedback model)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();
                Feedback feedback = db.Feedbacks.Where(x => x.Id == model.Id).FirstOrDefault();
                feedback.Status = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
                db.Feedbacks.Update(feedback);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Index");
        }
    }
}
