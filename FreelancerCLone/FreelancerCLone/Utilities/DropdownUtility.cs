using FreelancerCLone.Constants;
using FreelancerCLone.DbModels;
using FreelancerCLone.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreelancerCLone.Utilities
{
    public class DropdownUtility
    {

        private static DropdownUtility _instance;
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


        public List<SelectListItem> getSelectList(string userName)
        {
            var id = UserUtility.Instance.GetUserId(userName);
            List<SelectListItem> specificSkills = new List<SelectListItem>();
            FreelancerDbContext db = new FreelancerDbContext();
            var skills = db.Skills.Where(x => x.IsActive == true).ToList();
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