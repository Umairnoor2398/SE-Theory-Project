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

            User userDb = new User();

            try
            {
                FreelancerDbContext db = new FreelancerDbContext();
                if (user == 0)
                {
                    int userId = UserUtility.Instance.GetUserId(User.Identity.Name);
                    userDb = db.Users.Find(userId);
                }
                else
                {
                    userDb = db.Users.Find(user);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return View(userDb);
        }

        public IActionResult Projects(int user = 0)
        {
            List<FreelancerPersonalProject> freelancerProjects = new List<FreelancerPersonalProject>();
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();

                int userId = user;

                if (user == 0)
                {
                    userId = UserUtility.Instance.GetUserId(User.Identity.Name);
                }
                freelancerProjects = db.FreelancerPersonalProjects.Where(x => x.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return View(freelancerProjects);
        }

        public IActionResult CreateEditProject(int id = 0)
        {
            ViewBag.Id = id;
            if (id != 0)
            {
                try
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
                catch (Exception ex)
                {
                    ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEditProject(FreelancerPersonalProjectViewModel model)
        {

            try
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
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return RedirectToAction("Projects");
        }

        public IActionResult Skills(int user = 0)
        {
            List<UserSkill> userSkills = new List<UserSkill>();

            try
            {
                FreelancerDbContext db = new FreelancerDbContext();

                if (user == 0)
                {
                    user = UserUtility.Instance.GetUserId(User.Identity.Name);
                }

                userSkills = db.UserSkills.Where(x => x.UserId == user).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return View(userSkills);
        }


        public IActionResult AddUserSkills()
        {
            try
            {
                ViewBag.skills = DropdownUtility.Instance.getSelectList(User.Identity.Name);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return PartialView("UserSkillsCreateEditPartialView");
        }

        [HttpPost]
        public IActionResult AddUserSkills(UserSkill model)
        {
            try
            {
                model.UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
                model.AddedOn = DateTime.Now;
                model.UpdatedOn = DateTime.Now;
                model.IsActive = true;
                FreelancerDbContext db = new FreelancerDbContext();
                db.UserSkills.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Skills");
        }

        public IActionResult DeleteUserSkills(int Id)
        {
            try
            {
                FreelancerDbContext db = new FreelancerDbContext();

                var skill = db.UserSkills.Find(Id);
                skill.IsActive = false;
                db.UserSkills.Update(skill);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Skills");
        }
    }
}
