using FreelancerCLone.DbModels;
using FreelancerCLone.ViewModels;

namespace FreelancerCLone.Utilities
{
	// Utility class for managing skills and skill categories in the FreelancerClone application
	public class SkillsUtility
	{
		private static SkillsUtility _instance;

		// Singleton pattern: Ensures only one instance of the SkillsUtility is created
		public static SkillsUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new SkillsUtility();
				return _instance;
			}
		}

		private SkillsUtility() { }

		// Retrieves a list of skills belonging to a specific category
		public List<Skill> GetSkillsOfCategory(int category)
		{
			List<Skill> skills;
            FreelancerDbContext db = new FreelancerDbContext();
            skills = db.Skills.Where(x => x.SkillCategoryId == category && x.IsActive == true).ToList();
            return skills;
        }

		// Retrieves a list of all skill categories
		public List<SkillCategory> GetSkillCategories()
        {
            List<SkillCategory> skillCategories;
            FreelancerDbContext db = new FreelancerDbContext();
            skillCategories = db.SkillCategories.Where(x => x.IsActive == true).ToList();
            return skillCategories;
        }
	
        // Retrieves a skill category by its Id
		public SkillCategory GetCategoryById(int id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skillCategory = db.SkillCategories.Find(id);

            if (skillCategory == null)
            {
                throw new Exception("Category not found");
            }

            return skillCategory;
        }
		// Adds a new skill category to the database
		public void AddSkillCategory(SkillCategory model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            SkillCategory skillCategory = new SkillCategory();
            skillCategory.CategoryName = model.CategoryName;
            skillCategory.AddedOn = DateTime.Now;
            skillCategory.UpdatedOn = DateTime.Now;
            skillCategory.IsActive = true;

            db.SkillCategories.Add(skillCategory);
            db.SaveChanges();
        }

		// Edits an existing skill category in the database
		public void EditSkillCategory(SkillCategory model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skillCategory = db.SkillCategories.Find(model.Id);
            skillCategory.CategoryName = model.CategoryName;
            skillCategory.UpdatedOn = DateTime.Now;
            db.SkillCategories.Update(skillCategory);
            db.SaveChanges();
        }

		// Retrieves a skill by its ID
		public Skill GetSkillById(int skillId)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skill = db.Skills.Find(skillId);
            if (skill == null)
            {
                throw new Exception("Skill not found");
            }
            return skill;
        }
		// Adds a new skill to the database
		public void AddSkill(Skill model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            Skill skill = new Skill();
            skill.SkillName = model.SkillName;
            skill.SkillCategoryId = model.SkillCategoryId;
            skill.AddedOn = DateTime.Now;
            skill.UpdatedOn = DateTime.Now;
            skill.IsActive = true;

            db.Skills.Add(skill);
            db.SaveChanges();
        }
		// Edits an existing skill in the database
		public void EditSkill(Skill model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skill = db.Skills.Find(model.Id);
            skill.SkillName = model.SkillName;
            skill.UpdatedOn = DateTime.Now;
            db.Skills.Update(skill);

            db.SaveChanges();
        }
		// Deletes a skill by its ID
		public Skill DeleteSkillById(int Id)
        {
            Skill skill;
            FreelancerDbContext db = new FreelancerDbContext();
            skill = db.Skills.Find(Id);

            skill.IsActive = !skill.IsActive;

            db.Skills.Update(skill);
            db.SaveChanges();
            return skill;
        }
		// Deletes or restores a skill category by its ID
		public void DeleteCategoryById(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var skillCategory = db.SkillCategories.Find(Id);

            skillCategory.IsActive = !skillCategory.IsActive;

            db.SkillCategories.Update(skillCategory);
            db.SaveChanges();
        }

    }
}
