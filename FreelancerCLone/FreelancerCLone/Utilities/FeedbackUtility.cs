using Microsoft.AspNetCore.Mvc.Rendering;
using FreelancerCLone.DbModels;
using FreelancerCLone.Constants;

namespace FreelancerCLone.Utilities
{

    public class FeedbackUtility
    {
        private static FeedbackUtility _instance;

        public static FeedbackUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FeedbackUtility();
                return _instance;
            }
        }

        private FeedbackUtility() { }

        public List<Feedback> GetPendingFeedbacks()
        {
            List<Feedback> feedbacks;
            FreelancerDbContext db = new FreelancerDbContext();
            feedbacks = db.Feedbacks.Where(x => x.Status == db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id).ToList();
            return feedbacks;
        }

        public void CreateFeedback(Feedback model, string username)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            model.AddedOn = DateTime.Now;
            model.Status = db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id;
            model.AddedBy = UserUtility.Instance.GetUserId(username);
            db.Feedbacks.Add(model);
            db.SaveChanges();
        }
        public Feedback GetFeedback(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            Feedback model = db.Feedbacks.Where(x => x.Id == Id).FirstOrDefault();
            return model;
        }
        public Feedback UpdateFeedbackStatusToAccept(Feedback model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            Feedback feedback = db.Feedbacks.Where(x => x.Id == model.Id).FirstOrDefault();
            feedback.Status = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
            db.Feedbacks.Update(feedback);
            db.SaveChanges();

            return feedback;
        }

    }

}
