using FreelancerCLone.Constants;
using FreelancerCLone.Utilities;

namespace FreelancerCLone.Services
{
	// Service class responsible for handling file uploads in the FreelancerClone application
	public class UploadFileService
	{
		private static UploadFileService _instance;

		// Singleton pattern: Ensures only one instance of the UploadFileService is created
		public static UploadFileService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UploadFileService();
				return _instance;
			}
		}

		private UploadFileService() { }

		// Uploads a file to the specified folder path and returns the file's relative web path
		public async Task<string> UploadFile(IFormFile image, List<FilePathEnum> folderPath, IWebHostEnvironment webHost)
		{
			// Get the root path of the web server
			var webRootPath = webHost.WebRootPath;
			string path = "";
			string path2 = "";
			foreach (var fPath in folderPath)
			{
				path = Path.Combine(path, fPath.ToString());
			}
			path2 = path;
			var WebFolder = Path.Combine(webRootPath, path);
			if (!Directory.Exists(WebFolder))
			{
				Directory.CreateDirectory(WebFolder);
			}
			// Get the absolute path of the images folder
			var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
			string uniquefileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
			var fileName = Path.Combine(imagesFolder, uniquefileName);
			string uploadFilePath = Path.Combine(Directory.GetCurrentDirectory(), WebFolder, fileName);
			using (FileStream file = new FileStream(uploadFilePath, FileMode.Create))
			{
				await image.CopyToAsync(file);
			}

			// Combine the relative folder path and the unique file name to get the file's relative web path
			string filePath = "/" + Path.Combine(path2, uniquefileName);
			return filePath;
		}
	}
}
