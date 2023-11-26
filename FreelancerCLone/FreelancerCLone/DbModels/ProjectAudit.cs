using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class ProjectAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Budget { get; set; }
        public int? Deadline { get; set; }
        public string? TechnologyRequired { get; set; }
        public int? Status { get; set; }
        public int? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsAssigned { get; set; }
        public bool? IsActive { get; set; }
    }
}
