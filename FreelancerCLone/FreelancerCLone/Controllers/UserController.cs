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
                userDb = UserUtility.Instance.GetUserForProfile(user, User.Identity.Name);
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
                freelancerProjects = UserUtility.Instance.GetFreelancerPersonalProjects(user, User.Identity.Name);
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
                    FreelancerPersonalProjectViewModel viewModel = UserUtility.Instance.GetFreelancerPersonalProjectViewModel(id);

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
                if (model.Id == 0)
                {
                    await UserUtility.Instance.AddFreelancerPersonalProject(model, User.Identity.Name, _webHost);
                }
                else
                {
                    await UserUtility.Instance.EditFreelancerPersonalProject(model, User.Identity.Name, _webHost);
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
                userSkills = UserUtility.Instance.GetUserSkills(user, User.Identity.Name);
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
                UserUtility.Instance.AddUserSkill(model);
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
                UserUtility.Instance.DeleteUserSkill(Id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Skills");
        }

        public IActionResult DeleteUserpersonalProjects(int Id)
        {
            try
            {
                UserUtility.Instance.DeleteUserSkill(Id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Skills");
        }

        public IActionResult ProjectDetails(int Id)
        {

            return View();
        }

    }
}
