using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class UserSkill
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SkillId { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Skill Skill { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
