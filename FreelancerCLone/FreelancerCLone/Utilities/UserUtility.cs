using FreelancerCLone.DbModels;
using FreelancerCLone.ViewModels;

namespace FreelancerCLone.Utilities
{
	public class UserUtility
	{
		private static UserUtility _instance;

		public static UserUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UserUtility();
				return _instance;
			}

		}

		public int GetUserId(string username)
		{
			FreelancerDbContext db = new FreelancerDbContext();
			var _user = db.AspNetUsers.Where(x => x.UserName == username).FirstOrDefault();
			return _user.Users.FirstOrDefault().Id;
		}

		public void AddUser(UserViewModel user)
		{
			FreelancerDbContext db = new FreelancerDbContext();
			User u = new User();
			u.FirstName = user.FirstName;
			u.Id = user.Id;
			u.LastName = user.LastName;
			u.AddedOn = DateTime.Now;
			u.UserId = user.UserId;
			u.ShortDescription = user.ShortDescription;
			u.LongDescription = user.LongDescription;
			u.IsActive = user.IsActive;
			u.ProfileImagePath = user.ProfileImagePath;
			u.Status = user.Status;
			db.Users.Add(u);
			db.SaveChanges();


		}

	}
}
