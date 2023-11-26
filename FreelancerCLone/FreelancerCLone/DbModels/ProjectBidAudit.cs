using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class ProjectBidAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public int? BidBudget { get; set; }
        public int? BidDeadline { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int? Status { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsReviewed { get; set; }
        public double? Rating { get; set; }
    }
}
