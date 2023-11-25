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

		// Constructor for ProjectController, initializes the web host environment
		public ProjectController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		// Displays a list of projects based on user and search query
		public IActionResult Index(string query)
		{
			List<ProjectViewModel> projects = new List<ProjectViewModel>();
			try
			{
				// Fetches projects based on user and filters by search query
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
				// Log any exceptions that occur during project retrieval
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return View(projects);
		}

		// Displays projects assigned to the currently authenticated user
		[Authorize]
		public IActionResult AssignedProjects()
		{
			List<ProjectBid> userBids = new List<ProjectBid>();
			try
			{
				// Retrieves projects assigned to the authenticated user
				int UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
				userBids = ProjectUtility.Instance.GetUserBids(UserId);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during fetching assigned projects
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return View(userBids);
		}

		// Displays projects added by the currently authenticated user
		[Authorize]
		public IActionResult MyProjects()
		{
			List<ProjectViewModel> userprojects = new List<ProjectViewModel>();
			try
			{
				// Retrieves projects added by the authenticated user
				int UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
				userprojects = ProjectUtility.Instance.GetUserAddedProjects(UserId);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during fetching user projects
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return View(userprojects);
		}

		// Displays a partial view to rate a user's bid on a project
		public IActionResult UserBidRate(int Id)
		{
			ViewBag.BidId = Id;
			return PartialView("UserBidRatePartialView");
		}

		// Handles the post request to rate a user's bid on a project
		[HttpPost]
		public async Task<IActionResult> BidRatePostAsync(ProjectBid model)
		{
			try
			{
				// Rates a user's bid on a project and sends a notification email
				var bid = ProjectUtility.Instance.RateUserProjectBid(model);
				await MailSenderService.Instance.SendMailRateProject(bid);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during rating a bid
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("MyProjects");
		}

		// Displays details of a specific project
		public IActionResult Details(int Id)
		{
			Project project = new Project();
			try
			{
				// Retrieves details of a specific project
				project = ProjectUtility.Instance.GetProject(Id);

				ViewBag.ApprovedId = LookupUtility.Instance.getId("Accepted");
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during fetching project details
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return View(project);
		}

		// Displays a partial view to bid on a project
		public IActionResult BidProject(int Id)
		{
			ViewBag.ProjectId = Id;
			return PartialView("ProjectBidPartialView");
		}

		// Handles the post request to bid on a project
		[HttpPost]
		public async Task<IActionResult> BidProjectPostAsync(ProjectBid model)
		{
			try
			{
				// Adds a bid to a project and sends a notification email
				model = ProjectUtility.Instance.AddProjectBid(model, User.Identity.Name);
				await MailSenderService.Instance.SendMailOnReceiveBidInProject(model);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during bidding on a project
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}

			return RedirectToAction("Details", new { Id = model.ProjectId });
		}

		// Approves a bid on a project and sends a notification email
		public async Task<IActionResult> ApproveBidAsync(int BidId, int ProjectId)
		{
			try
			{
				// Approves a bid on a project and sends a notification email
				var bid = ProjectUtility.Instance.ApproveUserProjectBid(BidId);
				await MailSenderService.Instance.SendMailOnApproveBidInProject(bid);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during approving a bid
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}

			return RedirectToAction("Details", new { Id = ProjectId });
		}

		// Deletes a user's bid on a project
		public IActionResult DeleteBid(int Id)
		{
			try
			{
				// Deletes a user's bid on a project
				ProjectUtility.Instance.DeleteUserProjectBid(Id);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during deleting a bid
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("AssignedProjects");
		}

		// Changes the completeness status of a project and sends a notification email
		public async Task<IActionResult> ChangeProjectCompletenessAsync(int Id)
		{
			try
			{
				// Changes the completeness status of a project and sends a notification email
				var bid = ProjectUtility.Instance.UpdateProjectCompleteStatus(Id);
				await MailSenderService.Instance.SendMailOnProjectCompletenesssUpdate(bid);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during changing project completeness status
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("AssignedProjects");
		}

		// Displays the project creation view
		[Authorize]
		public IActionResult Create(int Id = 0)
		{
			ViewBag.Id = Id;
			if (Id != 0)
			{
				try
				{
					// Retrieves project details for editing
					ProjectViewModel viewModel = ProjectUtility.Instance.GetProjectViewModel(Id);
					return View(viewModel);
				}
				catch (Exception ex)
				{
					// Log any exceptions that occur during fetching project details for editing
					ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				}
			}
			return View();
		}

		// Handles the post request for creating or updating a project
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create(ProjectViewModel project)
		{
			try
			{
				// Adds or updates a project and sends a notification email


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
				// Log any exceptions that occur during project creation or update
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("MyProjects");
		}

		// Deletes a project
		public async Task<IActionResult> Delete(int Id)
		{
			try
			{
				// Deletes a project
				ProjectUtility.Instance.DeleteProject(Id);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during project deletion
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return RedirectToAction("MyProjects");
		}
	}
}
