using Microsoft.AspNetCore.Mvc.Rendering;
using FreelancerCLone.DbModels;
using FreelancerCLone.Constants;

namespace FreelancerCLone.Utilities
{
	// Utility class for managing lookup-related operations in the FreelancerClone application
	public class LookupUtility
	{
		private static LookupUtility _instance;

		// Singleton pattern: Ensures only one instance of the LookupUtility is created
		public static LookupUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new LookupUtility();
				return _instance;
			}
		}

		private LookupUtility() { }

		// Retrieves a list of SelectListItem based on the specified lookup category
		public List<SelectListItem> getSelectList(LookupCategory category)
		{
			FreelancerDbContext db = new FreelancerDbContext();
			var lst = db.Lookups
				.Where(x => x.Category.ToUpper() == category.ToString().ToUpper())
				.Select(a => new SelectListItem()
				{
					Text = a.Value,
					Value = a.Id + ""
				}).ToList();

			return lst;
		}

		// Retrieves the ID of a lookup entry based on its value
		public int getId(string value)
		{
			FreelancerDbContext db = new FreelancerDbContext();
			int id = db.Lookups.Where(x => x.Value == value).FirstOrDefault().Id;

			return id;
		}

		// Retrieves the value of a lookup entry based on its ID
		public string getValue(int id)
		{
			if (id == 0)
			{
				return "";
			}
			FreelancerDbContext db = new FreelancerDbContext();
			string value = db.Lookups.Where(x => x.Id == id).FirstOrDefault().Value;

			return value;
		}
	}
}
