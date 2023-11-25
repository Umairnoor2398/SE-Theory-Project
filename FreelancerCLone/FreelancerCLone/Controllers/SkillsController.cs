using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerCLone.Controllers
{
    public class SkillsController : Controller
    {

        public IActionResult Index(int category)
        {
            if (category != 0)
            {
                List<Skill> skills = new List<Skill>();
                ViewBag.CategoryId = category;
                try
                {
                    var c = SkillsUtility.Instance.GetCategoryById(category);
                    ViewBag.CategoryName = c.CategoryName;
                    skills = SkillsUtility.Instance.GetSkillsOfCategory(category);
                }
                catch (Exception ex)
                {
                    ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                }
                return View(skills);
            }
            return View();
        }



        public IActionResult Categories()
        {
            List<SkillCategory> skillCategories = new List<SkillCategory>();
            try
            {
                skillCategories = SkillsUtility.Instance.GetSkillCategories();
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return View(skillCategories);
        }



        public IActionResult CreateCategory(int id = 0)
        {
            ViewBag.Id = id;
            if (id != 0)
            {
                try
                {
                    SkillCategory skillCategory = SkillsUtility.Instance.GetCategoryById(id);
                    return PartialView("SkillsCategoryCreateEditPartialView", skillCategory);
                }
                catch (Exception ex)
                {
                    ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                }
            }
            return PartialView("SkillsCategoryCreateEditPartialView");
        }



        [HttpPost]
        public IActionResult CreateCategory(SkillCategory model)
        {
            try
            {
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
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Categories");
        }



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
                    Skill skill = SkillsUtility.Instance.GetSkillById(skillId);
                    return PartialView("SkillsCreateEditPartialView", skill);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return PartialView("SkillsCreateEditPartialView");
        }



        [HttpPost]
        public IActionResult CreateSkill(Skill model)
        {
            try
            {
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
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Index", new { category = model.SkillCategoryId });
        }



        public IActionResult DeleteCategory(int Id)
        {
            try
            {
                SkillsUtility.Instance.DeleteCategoryById(Id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Categories");
        }



        public IActionResult DeleteSkill(int Id)
        {
            Skill skill = new Skill();

            try
            {
                skill = SkillsUtility.Instance.DeleteSkillById(Id);
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
            }
            return RedirectToAction("Index", new { category = skill.SkillCategoryId });
        }


    }
}
