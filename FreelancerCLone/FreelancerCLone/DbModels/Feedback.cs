using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Message { get; set; } = null!;
        public int Status { get; set; }
        public DateTime AddedOn { get; set; }
        public int AddedBy { get; set; }

        public virtual User AddedByNavigation { get; set; } = null!;
        public virtual Lookup Category { get; set; } = null!;
        public virtual Lookup StatusNavigation { get; set; } = null!;
    }
}
