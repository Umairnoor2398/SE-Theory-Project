namespace FreelancerCLone.ViewModels
{
    public class UserViewModel
    {
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

        public IFormFile profileImage { get; set; }

    }
}
