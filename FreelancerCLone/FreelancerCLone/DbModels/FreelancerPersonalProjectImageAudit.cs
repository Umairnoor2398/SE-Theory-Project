using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class FreelancerPersonalProjectImageAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public int? PersonalProjectId { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
