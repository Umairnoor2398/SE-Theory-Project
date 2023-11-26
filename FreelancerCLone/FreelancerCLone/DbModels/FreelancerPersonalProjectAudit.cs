using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class FreelancerPersonalProjectAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Technology { get; set; }
        public string? PublicUrl { get; set; }
        public int? UserId { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
