using NLog;
using System.Runtime.CompilerServices;

namespace FreelancerCLone.Services
{
    public class ErrorLogger
    {
        private static ErrorLogger _instance;

        public static ErrorLogger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ErrorLogger();
                return _instance;
            }
        }
        public void ErrorLoggingFunction(string errorMessage, string className, [CallerMemberName] string callerMethodName = null)
        {
            LogManager.GetCurrentClassLogger().Error($"{errorMessage} occurred in {className}'s {callerMethodName} method.");
        }
    }
}
