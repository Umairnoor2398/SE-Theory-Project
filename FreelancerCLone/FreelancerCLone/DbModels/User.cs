using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class User
    {
        public User()
        {
            Feedbacks = new HashSet<Feedback>();
            FreelancerPersonalProjects = new HashSet<FreelancerPersonalProject>();
            ProjectBids = new HashSet<ProjectBid>();
            Projects = new HashSet<Project>();
            UserSkills = new HashSet<UserSkill>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public int Status { get; set; }

        public virtual Lookup StatusNavigation { get; set; } = null!;
        public virtual AspNetUser UserNavigation { get; set; } = null!;
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<FreelancerPersonalProject> FreelancerPersonalProjects { get; set; }
        public virtual ICollection<ProjectBid> ProjectBids { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
