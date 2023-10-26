using Microsoft.AspNetCore.Mvc.Rendering;
using FreelancerCLone.DbModels;
using FreelancerCLone.Constants;

namespace FreelancerCLone.Utilities
{

    public class LookupUtility
    {
        private static LookupUtility _instance;

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

        public List<SelectListItem> getSelectList(LookupCategory category)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var lst = db.Lookups.Where(x => x.Category.ToUpper() == category.ToString().ToUpper()).Select(a => new SelectListItem()
            {
                Text = a.Value,
                Value = a.Id + ""
            }).ToList();
            return lst;
        }

        public int getId(string value)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            int id = db.Lookups.Where(x => x.Value == value).FirstOrDefault().Id;
            return id;
        }


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
