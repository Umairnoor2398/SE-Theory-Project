using Microsoft.AspNetCore.Mvc.Rendering;
using FreelancerCLone.DbModels;
using FreelancerCLone.Constants;

namespace FreelancerCLone.Utilities
{

    public class FeedbackUtility
    {
        private static FeedbackUtility _instance;

        public static FeedbackUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FeedbackUtility();
                return _instance;
            }
        }

        private FeedbackUtility() { }


    }

}
