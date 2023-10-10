using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class FreelancerPersonalProject
    {
        public FreelancerPersonalProject()
        {
            FreelancerPersonalProjectImages = new HashSet<FreelancerPersonalProjectImage>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Technology { get; set; } = null!;
        public string? PublicUrl { get; set; }
        public int UserId { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<FreelancerPersonalProjectImage> FreelancerPersonalProjectImages { get; set; }
    }
}
