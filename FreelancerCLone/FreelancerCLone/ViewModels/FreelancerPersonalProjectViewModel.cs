using FreelancerCLone.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FreelancerCLone.ViewModels
{
    public class FreelancerPersonalProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public string Technology { get; set; } = null!;
        public string? PublicUrl { get; set; }
        public int UserId { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<FreelancerPersonalProjectImage> FreelancerPersonalProjectImages { get; set; }


        public IFormFileCollection images { get; set; }

    }
}
