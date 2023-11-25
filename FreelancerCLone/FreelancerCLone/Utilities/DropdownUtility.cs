using FreelancerCLone.Constants;
using FreelancerCLone.DbModels;
using FreelancerCLone.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreelancerCLone.Utilities
{
	// Utility class for managing dropdown lists in the FreelancerClone application
	public class DropdownUtility
	{
		private static DropdownUtility _instance;

		// Singleton pattern: Ensures only one instance of the DropdownUtility is created
		private DropdownUtility() { }

		public static DropdownUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new DropdownUtility();
				return _instance;
			}
		}

		// Retrieves a list of SelectListItem for a dropdown based on user-specific skills
		public List<SelectListItem> getSelectList(string userName)
		{
			var id = UserUtility.Instance.GetUserId(userName);
			List<SelectListItem> specificSkills = new List<SelectListItem>();
			FreelancerDbContext db = new FreelancerDbContext();

			// Retrieve all active skills from the database
			var skills = db.Skills.Where(x => x.IsActive == true).ToList();

			// Iterate through each skill and check if it is associated with the user
			foreach (var s in skills)
			{
				bool isFound = false;
				foreach (var sk in s.UserSkills)
				{
					if (sk.UserId == id)
					{
						isFound = true;
						break;
					}
				}

				// If the skill is not associated with the user, add it to the SelectListItem list
				if (isFound == false)
				{
					SelectListItem skillItem = new SelectListItem()
					{
						Text = s.SkillName,
						Value = s.Id + ""
					};

					specificSkills.Add(skillItem);
				}
			}
			return specificSkills;
		}
	}
}
