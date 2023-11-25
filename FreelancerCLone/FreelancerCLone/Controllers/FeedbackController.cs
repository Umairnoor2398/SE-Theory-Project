using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerCLone.Controllers
{
	public class FeedbackController : Controller
	{

		// Displays a list of pending feedbacks on the Index page
		public IActionResult Index()
		{
			List<Feedback> feedbacks = new List<Feedback>();
			try
			{
				feedbacks = FeedbackUtility.Instance.GetPendingFeedbacks();
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during fetching feedbacks
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return View(feedbacks);
		}



		// Handles the creation of new feedback
		[HttpPost]
		public async Task<IActionResult> Create(Feedback model)
		{
			try
			{
				// Creates feedback and sends a notification email
				FeedbackUtility.Instance.CreateFeedback(model, User.Identity.Name);
				await MailSenderService.Instance.SendMailToOnReceivingFeedback(User.Identity.Name);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during feedback creation
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("Index", "Home");
		}



		// Displays details of a specific feedback
		public IActionResult Details(int Id)
		{
			try
			{
				// Retrieves feedback details for a specific Id
				Feedback model = FeedbackUtility.Instance.GetFeedback(Id);
				return PartialView("FeedbackDetailsPartialView", model);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during fetching feedback details
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("Index");
		}



		// Updates the status of a feedback to 'Accepted' and sends a notification email
		public async Task<IActionResult> UpdateStatus(Feedback model)
		{
			try
			{
				// Updates feedback status and sends a notification email
				var feedback = FeedbackUtility.Instance.UpdateFeedbackStatusToAccept(model);
				await MailSenderService.Instance.SendMailToOnReviewingFeedback(feedback.AddedBy);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during updating feedback status
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("Index");
		}

	}
}
