using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class ProjectDocument
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string DocumentPath { get; set; } = null!;
        public string? DocumentType { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual Project Project { get; set; } = null!;
    }
}
