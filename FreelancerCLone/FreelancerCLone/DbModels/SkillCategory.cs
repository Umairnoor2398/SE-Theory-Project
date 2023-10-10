using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class SkillCategory
    {
        public SkillCategory()
        {
            Skills = new HashSet<Skill>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
