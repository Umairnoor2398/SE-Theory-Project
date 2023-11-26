using Microsoft.AspNetCore.Mvc;
using FreelancerCLone.ViewModels;
using FreelancerCLone.Utilities;
using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FreelancerCLone.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _webHost;

        // Constructor for UserController, initializes the web host environment
        public UserController(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        [Authorize]
        // Displays the user profile with the option to view/edit
        public IActionResult Profile(int user = 0)
        {
            User userDb = new User();

            try
            {
                // Retrieves user information for the profile
                userDb = UserUtility.Instance.GetUserForProfile(user, User.Identity.Name);
                bool isMyprofile = user == 0 ? true : false;
                ViewBag.isMyprofile = isMyprofile;

                ViewBag.AverageRating = userDb.ProjectBids.Where(x => x.IsReviewed == true).ToList().Average(x => x.Rating);


            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user profile retrieval
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return View(userDb);
        }
        [Authorize]
        // Displays the partial view for editing the user profile
        public IActionResult Editprofile()
        {
            UserViewModel userDb = new UserViewModel();
            try
            {
                // Retrieves user information for editing the profile
                userDb = UserUtility.Instance.GetUserViewModl(User.Identity.Name);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user profile editing retrieval
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return PartialView("ProfileEditPartialView", userDb);
        }
        [Authorize]
        // Handles the post request to update the user profile
        public async Task<IActionResult> EditprofilePost(UserViewModel user)
        {
            await UserUtility.Instance.UpdateProfileAsync(user, User.Identity.Name, _webHost);

            return RedirectToAction("Profile");
        }
        [Authorize]
        // Displays the user's projects
        public IActionResult Projects(int user = 0)
        {
            List<FreelancerPersonalProject> freelancerProjects = new List<FreelancerPersonalProject>();
            try
            {
                // Retrieves and displays the user's personal projects
                freelancerProjects = UserUtility.Instance.GetFreelancerPersonalProjects(user, User.Identity.Name);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during personal project retrieval
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return View(freelancerProjects);
        }
        [Authorize]
        // Displays the partial view for creating or editing a personal project
        public IActionResult CreateEditProject(int id = 0)
        {
            ViewBag.Id = id;
            if (id != 0)
            {
                try
                {
                    // Retrieves and displays the personal project for editing
                    FreelancerPersonalProjectViewModel viewModel = UserUtility.Instance.GetFreelancerPersonalProjectViewModel(id);

                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during personal project retrieval for editing
                    ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                }
            }
            return View();
        }
        [Authorize]
        // Handles the post request for creating or editing a personal project
        [HttpPost]
        public async Task<IActionResult> CreateEditProject(FreelancerPersonalProjectViewModel model)
        {

            try
            {
                // Adds or edits a personal project
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
                // Log any exceptions that occur during personal project creation or update
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return RedirectToAction("Projects");
        }
        [Authorize]
        // Displays the user's skills
        public IActionResult Skills(int user = 0)
        {
            List<UserSkill> userSkills = new List<UserSkill>();

            try
            {
                // Retrieves and displays the user's skills
                userSkills = UserUtility.Instance.GetUserSkills(user, User.Identity.Name);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user skill retrieval
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }

            return View(userSkills);
        }
        [Authorize]
        // Displays the partial view for adding user skills
        public IActionResult AddUserSkills()
        {
            try
            {
                // Retrieves and displays a list of skill categories for selection
                List<SelectListItem> categories = DropdownUtility.Instance.getSkillCategories();
                ViewBag.skillCategories = categories;
                // Retrieves and displays a list of skills for selection
                ViewBag.skills = DropdownUtility.Instance.getSkillsSelectList(User.Identity.Name, int.Parse(categories.FirstOrDefault().Value));
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during skill dropdown retrieval
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return PartialView("UserSkillsCreateEditPartialView");
        }
        [Authorize]
        public IActionResult GetSkills(int categoryId)
        {
            IEnumerable<SelectListItem> skillsDropdown = DropdownUtility.Instance.getSkillsSelectList(User.Identity.Name, categoryId);
            return Json(skillsDropdown);
        }
        [Authorize]
        // Handles the post request for adding user skills
        [HttpPost]
        public IActionResult AddUserSkills(UserSkill model)
        {
            try
            {
                // Adds a user skill
                model.UserId = UserUtility.Instance.GetUserId(User.Identity.Name);
                model.AddedOn = DateTime.Now;
                model.UpdatedOn = DateTime.Now;
                model.IsActive = true;
                UserUtility.Instance.AddUserSkill(model);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user skill addition
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Skills");
        }
        [Authorize]
        // Deletes a user skill
        public IActionResult DeleteUserSkills(int Id)
        {
            try
            {
                // Deletes a user skill
                UserUtility.Instance.DeleteUserSkill(Id);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user skill deletion
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Skills");
        }
        [Authorize]
        // Deletes a user's personal project
        public IActionResult DeleteUserpersonalProjects(int Id)
        {
            try
            {
                // Deletes a user's personal project
                UserUtility.Instance.DeleteUserSkill(Id);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during user personal project deletion
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Skills");
        }
        [Authorize]
        // Displays the details of a personal project
        public IActionResult ProjectDetails(int Id)
        {
            return View();
        }
    }
}
