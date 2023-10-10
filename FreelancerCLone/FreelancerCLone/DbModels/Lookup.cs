using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class Lookup
    {
        public Lookup()
        {
            FeedbackCategories = new HashSet<Feedback>();
            FeedbackStatusNavigations = new HashSet<Feedback>();
            ProjectBids = new HashSet<ProjectBid>();
            Projects = new HashSet<Project>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int? DisplayOrder { get; set; }

        public virtual ICollection<Feedback> FeedbackCategories { get; set; }
        public virtual ICollection<Feedback> FeedbackStatusNavigations { get; set; }
        public virtual ICollection<ProjectBid> ProjectBids { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
