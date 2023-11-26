using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerCLone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SkillsController : Controller
    {

        // Displays the skills of a specific category
        public IActionResult Index(int category)
        {
            if (category != 0)
            {
                List<Skill> skills = new List<Skill>();
                ViewBag.CategoryId = category;
                try
                {
                    // Retrieves and displays skills of the specified category
                    var c = SkillsUtility.Instance.GetCategoryById(category);
                    ViewBag.CategoryName = c.CategoryName;
                    skills = SkillsUtility.Instance.GetSkillsOfCategory(category);
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during skill retrieval
                    ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                }
                return View(skills);
            }
            return View();
        }

        // Displays all skill categories
        public IActionResult Categories()
        {
            List<SkillCategory> skillCategories = new List<SkillCategory>();
            try
            {
                // Retrieves and displays all skill categories
                skillCategories = SkillsUtility.Instance.GetSkillCategories();
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during skill category retrieval
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View(skillCategories);
        }

        // Displays the partial view for creating or editing a skill category
        public IActionResult CreateCategory(int id = 0)
        {
            ViewBag.Id = id;
            if (id != 0)
            {
                try
                {
                    // Retrieves and displays the skill category for editing
                    SkillCategory skillCategory = SkillsUtility.Instance.GetCategoryById(id);
                    return PartialView("SkillsCategoryCreateEditPartialView", skillCategory);
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during skill category retrieval for editing
                    ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                }
            }
            return PartialView("SkillsCategoryCreateEditPartialView");
        }

        // Handles the post request for creating or editing a skill category
        [HttpPost]
        public IActionResult CreateCategory(SkillCategory model)
        {
            try
            {
                // Adds or edits a skill category
                if (model.Id != 0)
                {
                    SkillsUtility.Instance.EditSkillCategory(model);
                }
                else
                {
                    SkillsUtility.Instance.AddSkillCategory(model);
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during skill category creation or update
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Categories");
        }

        // Displays the partial view for creating or editing a skill
        public IActionResult CreateSkill(string ID = "")
        {
            try
            {
                ID += " 0";
                var ids = ID.Split(" ");
                int skillId = Convert.ToInt32(ids[1]);

                ViewBag.Id = skillId;
                ViewBag.CategoryId = Convert.ToInt32(ids[0]);

                if (skillId != 0)
                {
                    // Retrieves and displays the skill for editing
                    Skill skill = SkillsUtility.Instance.GetSkillById(skillId);
                    return PartialView("SkillsCreateEditPartialView", skill);
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during skill retrieval for editing
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return PartialView("SkillsCreateEditPartialView");
        }

        // Handles the post request for creating or editing a skill
        [HttpPost]
        public IActionResult CreateSkill(Skill model)
        {
            try
            {
                // Adds or edits a skill
                if (model.Id != 0)
                {
                    SkillsUtility.Instance.EditSkill(model);
                }
                else
                {
                    SkillsUtility.Instance.AddSkill(model);
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during skill creation or update
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Index", new { category = model.SkillCategoryId });
        }

        // Deletes a skill category
        public IActionResult DeleteCategory(int Id)
        {
            try
            {
                // Deletes a skill category
                SkillsUtility.Instance.DeleteCategoryById(Id);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during skill category deletion
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Categories");
        }

        // Deletes a skill
        public IActionResult DeleteSkill(int Id)
        {
            Skill skill = new Skill();

            try
            {
                // Deletes a skill
                skill = SkillsUtility.Instance.DeleteSkillById(Id);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during skill deletion
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Index", new { category = skill.SkillCategoryId });
        }
    }
}
