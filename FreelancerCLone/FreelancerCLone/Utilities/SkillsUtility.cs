using FreelancerCLone.DbModels;
using FreelancerCLone.ViewModels;

namespace FreelancerCLone.Utilities
{
    public class SkillsUtility
    {
            private static SkillsUtility _instance;

            public static SkillsUtility Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new SkillsUtility();
                    return _instance;
                }


            }
    }
}
