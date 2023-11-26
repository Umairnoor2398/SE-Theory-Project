using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class UserAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public int? Status { get; set; }
    }
}
