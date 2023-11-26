using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class FeedbackAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public int? CategoryId { get; set; }
        public string? Message { get; set; }
        public int? Status { get; set; }
        public DateTime? AddedOn { get; set; }
        public int? AddedBy { get; set; }
    }
}
