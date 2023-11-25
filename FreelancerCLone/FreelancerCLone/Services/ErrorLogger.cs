using NLog;
using System.Runtime.CompilerServices;

namespace FreelancerCLone.Services
{
	
	// Singleton class for logging errors using NLog.
	public class ErrorLogger
	{
		// Singleton instance
		private static ErrorLogger _instance;


		// Gets the singleton instance of ErrorLogger.	
		public static ErrorLogger Instance
		{
			get
			{
				if (_instance == null)
					_instance = new ErrorLogger();
				return _instance;
			}
		}

		
		// Logs an error message along with the class name and method name where the error occurred.
		public void ErrorLoggingFunction(string errorMessage, string className, [CallerMemberName] string callerMethodName = null)
		{
			// Using NLog to log the error message along with class and method information.
			LogManager.GetCurrentClassLogger().Error($"{errorMessage} occurred in {className}'s {callerMethodName} method.");
		}
	}
}
