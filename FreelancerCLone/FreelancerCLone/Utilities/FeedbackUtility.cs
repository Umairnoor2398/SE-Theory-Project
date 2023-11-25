using Microsoft.AspNetCore.Mvc.Rendering;
using FreelancerCLone.DbModels;
using FreelancerCLone.Constants;

namespace FreelancerCLone.Utilities
{
	// Utility class for managing feedback-related operations in the FreelancerClone application
	public class FeedbackUtility
	{
		private static FeedbackUtility _instance;

		// Singleton pattern: Ensures only one instance of the FeedbackUtility is created
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

		// Retrieves a list of pending feedbacks from the database
		public List<Feedback> GetPendingFeedbacks()
		{
			List<Feedback> feedbacks;
			FreelancerDbContext db = new FreelancerDbContext();

			// Retrieve feedbacks with a status of "Pending"
			feedbacks = db.Feedbacks.Where(x => x.Status == db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id).ToList();

			return feedbacks;
		}

		// Creates a new feedback entry in the database
		public void CreateFeedback(Feedback model, string username)
		{
			FreelancerDbContext db = new FreelancerDbContext();
			model.AddedOn = DateTime.Now;

			// Set the feedback status to "Pending"
			model.Status = db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id;
			model.AddedBy = UserUtility.Instance.GetUserId(username);
			db.Feedbacks.Add(model);
			db.SaveChanges();
		}

		// Retrieves a specific feedback by its ID
		public Feedback GetFeedback(int Id)
		{
			FreelancerDbContext db = new FreelancerDbContext();
			Feedback model = db.Feedbacks.Where(x => x.Id == Id).FirstOrDefault();

			return model;
		}

		// Updates the status of a feedback to "Accepted"
		public Feedback UpdateFeedbackStatusToAccept(Feedback model)
		{
			FreelancerDbContext db = new FreelancerDbContext();
			Feedback feedback = db.Feedbacks.Where(x => x.Id == model.Id).FirstOrDefault();

			// Set the status to "Accepted"
			feedback.Status = db.Lookups.Where(x => x.Value == "Accepted").FirstOrDefault().Id;
			db.Feedbacks.Update(feedback);
			db.SaveChanges();

			return feedback;
		}
	}
}
