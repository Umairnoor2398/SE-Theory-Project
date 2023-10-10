using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class FreelancerPersonalProjectImage
    {
        public int Id { get; set; }
        public int PersonalProjectId { get; set; }
        public int ImagePath { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual FreelancerPersonalProject PersonalProject { get; set; } = null!;
    }
}
