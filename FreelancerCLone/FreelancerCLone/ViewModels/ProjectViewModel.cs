using FreelancerCLone.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FreelancerCLone.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Budget in $")]
        [Range(5, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Budget { get; set; }
        [Required]
        [Display(Name = "Deadline in Days")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? Deadline { get; set; }
        [Required]
        [Display(Name = "Technology/Skills Required")]
        public string TechnologyRequired { get; set; } = null!;
        public int Status { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual User AddedByNavigation { get; set; }
        public virtual ICollection<ProjectBid> ProjectBids { get; set; }
        public virtual ICollection<ProjectDocument> ProjectDocuments { get; set; }


        public IFormFileCollection docs { get; set; }

    }
}
