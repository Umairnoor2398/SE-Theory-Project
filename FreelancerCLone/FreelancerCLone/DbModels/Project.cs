using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class Project
    {
        public Project()
        {
            ProjectBids = new HashSet<ProjectBid>();
            ProjectDocuments = new HashSet<ProjectDocument>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Budget { get; set; }
        public int? Deadline { get; set; }
        public string TechnologyRequired { get; set; } = null!;
        public int Status { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsAssigned { get; set; }
        public bool? IsActive { get; set; }

        public virtual User AddedByNavigation { get; set; } = null!;
        public virtual Lookup StatusNavigation { get; set; } = null!;
        public virtual ICollection<ProjectBid> ProjectBids { get; set; }
        public virtual ICollection<ProjectDocument> ProjectDocuments { get; set; }
    }
}
