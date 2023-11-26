using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class ProjectDocumentAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public int? ProjectId { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentType { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
