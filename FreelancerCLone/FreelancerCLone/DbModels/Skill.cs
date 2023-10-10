using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class Skill
    {
        public Skill()
        {
            UserSkills = new HashSet<UserSkill>();
        }

        public int Id { get; set; }
        public string SkillName { get; set; } = null!;
        public int SkillCategoryId { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual SkillCategory SkillCategory { get; set; } = null!;
        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
