using FreelancerCLone.DbModels;

namespace FreelancerCLone.ViewModels
{
	public class ProjectViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public int Budget { get; set; }
		public int? Deadline { get; set; }
		public string TechnologyRequired { get; set; } = null!;
		public int Status { get; set; }
		public int AddedBy { get; set; }
		public DateTime AddedOn { get; set; }
		public DateTime? UpdatedOn { get; set; }
		public bool? IsActive { get; set; }


		public virtual ICollection<ProjectBid> ProjectBids { get; set; }
		public virtual ICollection<ProjectDocument> ProjectDocuments { get; set; }


		public IFormFileCollection docs { get; set; }

	}
}
