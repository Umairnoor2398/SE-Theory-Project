using FreelancerCLone.DbModels;
using FreelancerCLone.ViewModels;

namespace FreelancerCLone.Utilities
{
    public class UserSkillsUtility
    {
            private static UserSkillsUtility _instance;

            public static UserSkillsUtility Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new UserSkillsUtility();
                    return _instance;
                }


            }



    }
}
