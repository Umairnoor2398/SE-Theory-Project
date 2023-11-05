using FreelancerCLone.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerCLone.Controllers
{
    public class SkillsController : Controller
    {

        public IActionResult Index(int category = 0)
        {
            ViewBag.CategoryId = category;
            FreelancerDbContext db = new FreelancerDbContext();
            var skills = db.Skills.Where(x => x.SkillCategoryId == category).ToList();
            return View(skills);
        }

        public IActionResult Categories()
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skillsCategory = db.SkillCategories.ToList();
            return View(skillsCategory);
        }

        public IActionResult CreateCategory(int id = 0)
        {
            ViewBag.Id = id;
            if (id != 0)
            {
                FreelancerDbContext db = new FreelancerDbContext();
                var skillCategory = db.SkillCategories.Find(id);
                return PartialView("SkillsCategoryCreateEditPartialView", skillCategory);
            }
            return PartialView("SkillsCategoryCreateEditPartialView");
        }
        [HttpPost]
        public IActionResult CreateCategory(SkillCategory model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            if (model.Id != 0)
            {
                var skillCategory = db.SkillCategories.Find(model.Id);
                skillCategory.CategoryName = model.CategoryName;
                skillCategory.UpdatedOn = DateTime.Now;
                db.SkillCategories.Update(skillCategory);

            }
            else
            {
                SkillCategory skillCategory = new SkillCategory();
                skillCategory.CategoryName = model.CategoryName;
                skillCategory.AddedOn = DateTime.Now;
                skillCategory.UpdatedOn = DateTime.Now;
                skillCategory.IsActive = true;

                db.SkillCategories.Add(skillCategory);
            }
            db.SaveChanges();
            return RedirectToAction("Categories");
        }


        public IActionResult CreateSkill(string ID = "")
        {
            ID += " 0";
            var ids = ID.Split(" ");
            ViewBag.Id = Convert.ToInt32(ids[1]);
            ViewBag.CategoryId = Convert.ToInt32(ids[0]);
            if (Convert.ToInt32(ids[1]) != 0)
            {
                FreelancerDbContext db = new FreelancerDbContext();
                var skill = db.Skills.Find(Convert.ToInt32(ids[1]));
                return PartialView("SkillsCreateEditPartialView", skill);
            }
            return PartialView("SkillsCreateEditPartialView");
        }
        [HttpPost]
        public IActionResult CreateSkill(Skill model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            if (model.Id != 0)
            {
                var skill = db.Skills.Find(model.Id);
                skill.SkillName = model.SkillName;
                skill.UpdatedOn = DateTime.Now;
                db.Skills.Update(skill);

            }
            else
            {
                Skill skill = new Skill();
                skill.SkillName = model.SkillName;
                skill.SkillCategoryId = model.SkillCategoryId;
                skill.AddedOn = DateTime.Now;
                skill.UpdatedOn = DateTime.Now;
                skill.IsActive = true;

                db.Skills.Add(skill);
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { category = model.SkillCategoryId });
        }

        public IActionResult DeleteCategory(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skillCategory = db.SkillCategories.Find(Id);

            skillCategory.IsActive = !skillCategory.IsActive;

            db.SkillCategories.Update(skillCategory);
            db.SaveChanges();
            return RedirectToAction("Categories");
        }

        public IActionResult DeleteSkill(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skill = db.Skills.Find(Id);

            skill.IsActive = !skill.IsActive;

            db.Skills.Update(skill);
            db.SaveChanges();
            return RedirectToAction("Index", new { category = skill.SkillCategoryId });
        }
    }
}