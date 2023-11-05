using Microsoft.AspNetCore.Mvc;
using FreelancerCLone.ViewModels;
using FreelancerCLone.Utilities;
using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.Constants;

namespace FreelancerCLone.Controllers
{
	public class UserController : Controller
	{
		private readonly IWebHostEnvironment _webHost;

		public UserController(IWebHostEnvironment webHost)
		{
			_webHost = webHost;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Profile(int user = 0)
		{
			FreelancerDbContext db = new FreelancerDbContext();

			User userDb = new User();

			if (user == 0)
			{
				int userId = UserUtility.Instance.GetUserId(User.Identity.Name);
				userDb = db.Users.Find(userId);
			}
			else
			{
				userDb = db.Users.Find(user);
			}

			return View(userDb);
		}

		public IActionResult Projects(int user = 0)
		{
			FreelancerDbContext db = new FreelancerDbContext();

			int userId = UserUtility.Instance.GetUserId(User.Identity.Name);

			if (user != 0)
			{
				userId = user;
			}
			var projects = db.FreelancerPersonalProjects.Where(x => x.UserId == userId).ToList();

			return View(projects);
		}

		public IActionResult CreateEditProject(int id = 0)
		{
			ViewBag.Id = id;
			if (id != 0)
			{
				FreelancerDbContext db = new FreelancerDbContext();
				var project = db.FreelancerPersonalProjects.Find(id);

				FreelancerPersonalProjectViewModel viewModel = new FreelancerPersonalProjectViewModel();
				viewModel.Id = project.Id;
				viewModel.Title = project.Title;
				viewModel.Description = project.Description;
				viewModel.StartDate = project.StartDate;
				viewModel.EndDate = project.EndDate;
				viewModel.Technology = project.Technology;
				viewModel.PublicUrl = project.PublicUrl;
				viewModel.UserId = project.UserId;
				viewModel.AddedOn = project.AddedOn;
				viewModel.UpdatedOn = project.UpdatedOn;
				viewModel.IsActive = project.IsActive;
				viewModel.FreelancerPersonalProjectImages = project.FreelancerPersonalProjectImages;


				return View(viewModel);
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateEditProject(FreelancerPersonalProjectViewModel model)
		{

			FreelancerDbContext db = new FreelancerDbContext();
			if (model.Id == 0)
			{
				FreelancerPersonalProject proj = new FreelancerPersonalProject();
				proj.Title = model.Title;
				proj.Description = model.Description;
				proj.StartDate = model.StartDate;
				proj.EndDate = model.EndDate;
				proj.Technology = model.Technology;
				proj.PublicUrl = model.PublicUrl;
				proj.AddedOn = DateTime.Now;
				proj.UpdatedOn = DateTime.Now;
				proj.IsActive = true;

				proj.UserId = UserUtility.Instance.GetUserId(User.Identity.Name);

				db.FreelancerPersonalProjects.Add(proj);
				db.SaveChanges();

				List<FilePathEnum> path = new List<FilePathEnum>();

				path.Add(FilePathEnum.ProjectImages);

				foreach (var i in model.images)
				{
					FreelancerPersonalProjectImage img = new FreelancerPersonalProjectImage();
					img.PersonalProjectId = proj.Id;
					img.ImagePath = await UploadFileService.Instance.UploadFile(i, path, _webHost);
					img.AddedOn = DateTime.Now;
					img.UpdatedOn = DateTime.Now;
					img.IsActive = true;
					db.FreelancerPersonalProjectImages.Add(img);
				}

				db.SaveChanges();
			}
			else
			{
				var proj = db.FreelancerPersonalProjects.Find(model.Id);
				proj.Title = model.Title;
				proj.Description = model.Description;
				proj.StartDate = model.StartDate;
				proj.EndDate = model.EndDate;
				proj.Technology = model.Technology;
				proj.PublicUrl = model.PublicUrl;
				proj.UpdatedOn = DateTime.Now;

				db.FreelancerPersonalProjects.Update(proj);
				db.SaveChanges();

				List<FilePathEnum> path = new List<FilePathEnum>();

				path.Add(FilePathEnum.ProjectImages);

				foreach (var i in model.images)
				{
					FreelancerPersonalProjectImage img = new FreelancerPersonalProjectImage();
					img.PersonalProjectId = proj.Id;
					img.ImagePath = await UploadFileService.Instance.UploadFile(i, path, _webHost);
					img.AddedOn = DateTime.Now;
					img.UpdatedOn = DateTime.Now;
					img.IsActive = true;
					db.FreelancerPersonalProjectImages.Add(img);
				}

				db.SaveChanges();
			}




			return RedirectToAction("Projects");
		}

		public IActionResult Skills(int user = 0)
		{
			FreelancerDbContext db = new FreelancerDbContext();

			if (user == 0)
			{
				user = UserUtility.Instance.GetUserId(User.Identity.Name);
			}

			var skills = db.UserSkills.Where(x => x.UserId == user).ToList();

			return View(skills);
		}


		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(UserViewModel user)
		{
			UserUtility.Instance.AddUser(user);
			return RedirectToAction("Index");
		}
	}
}
