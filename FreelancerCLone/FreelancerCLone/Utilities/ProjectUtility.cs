using FreelancerCLone.Constants;
using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.ViewModels;

namespace FreelancerCLone.Utilities
{
    public class ProjectUtility
    {
        private static ProjectUtility _instance;

        public static ProjectUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ProjectUtility();
                return _instance;
            }


        }

        public async Task AddProject(ProjectViewModel project, string username, IWebHostEnvironment _webHost)
        {
            int userId = UserUtility.Instance.GetUserId(username);
            FreelancerDbContext db = new FreelancerDbContext();
            Project p = new Project();
            p.Deadline = project.Deadline;
            p.Description = project.Description;
            p.AddedOn = DateTime.Now;
            p.AddedBy = userId;
            p.Id = project.Id;
            p.IsActive = false;
            p.Status = db.Lookups.Where(x => x.Value == "Pending").FirstOrDefault().Id;
            p.TechnologyRequired = project.TechnologyRequired;
            p.Title = project.Title;
            p.Budget = project.Budget;
            db.Projects.Add(p);
            db.SaveChanges();


            List<FilePathEnum> path = new List<FilePathEnum>();

            path.Add(FilePathEnum.ProjectDocuments);

            if (project.docs != null)
            {
                foreach (var i in project.docs)
                {

                    ProjectDocument doc = new ProjectDocument();
                    doc.ProjectId = p.Id;
                    doc.DocumentPath = await UploadFileService.Instance.UploadFile(i, path, _webHost);
                    doc.DocumentType = CategorizeFile(i.FileName);
                    doc.AddedOn = DateTime.Now;
                    doc.UpdatedOn = DateTime.Now;
                    doc.IsActive = true;


                    db.ProjectDocuments.Add(doc);
                    db.SaveChanges();
                }
            }


        }

        static string CategorizeFile(string fileName)
        {
            // Get the file extension
            string fileExtension = Path.GetExtension(fileName).ToLower();

            // Categorize the file based on the extension
            switch (fileExtension)
            {
                case ".pdf":
                case ".doc":
                case ".docx":
                case ".xlsx":
                    return "Document";
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    return "Image";
                case ".mp4":
                case ".avi":
                case ".mov":
                    return "Video";
                default:
                    return "Other";
            }
        }

        public List<ProjectViewModel> GetProjects()
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var projects = db.Projects.ToList();
            List<ProjectViewModel> pro = new List<ProjectViewModel>();
            foreach (var p in projects)
            {
                ProjectViewModel viewModel = new ProjectViewModel();
                viewModel.Id = p.Id;
                viewModel.Title = p.Title;
                viewModel.Description = p.Description;
                viewModel.Deadline = p.Deadline;
                viewModel.IsActive = p.IsActive;
                viewModel.Status = p.Status;
                viewModel.TechnologyRequired = p.TechnologyRequired;
                viewModel.Budget = p.Budget;
                pro.Add(viewModel);
            }
            return pro;
        }

        public List<ProjectBid> GetUserBids(int UserId)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var userBids = db.ProjectBids.Where(x => x.UserId == UserId && x.IsActive == true).ToList();
            return userBids;
        }

        public List<ProjectViewModel> GetUserAddedProjects(int UserId)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var userProjects = db.Projects.Where(x => x.AddedBy == UserId).ToList();

            List<ProjectViewModel> pro = new List<ProjectViewModel>();
            foreach (var p in userProjects)
            {
                ProjectViewModel viewModel = new ProjectViewModel();
                viewModel.Id = p.Id;
                viewModel.Title = p.Title;
                viewModel.Description = p.Description;
                viewModel.Deadline = p.Deadline;
                viewModel.IsActive = p.IsActive;
                viewModel.Status = p.Status;
                viewModel.TechnologyRequired = p.TechnologyRequired;
                viewModel.Budget = p.Budget;
                viewModel.ProjectBids = p.ProjectBids;
                pro.Add(viewModel);
            }
            return pro;

        }

        public void UpdateProject(ProjectViewModel updatedProject)
        {
            using (FreelancerDbContext db = new FreelancerDbContext())
            {
                // Find the project by its unique ID
                Project existingProject = db.Projects.Find(updatedProject.Id);

                if (existingProject != null)
                {
                    // Update the properties of the existing project with new values
                    existingProject.Deadline = updatedProject.Deadline;
                    existingProject.Description = updatedProject.Description;
                    existingProject.IsActive = updatedProject.IsActive;
                    existingProject.Status = updatedProject.Status;
                    existingProject.TechnologyRequired = updatedProject.TechnologyRequired;
                    existingProject.Title = updatedProject.Title;
                    existingProject.Budget = updatedProject.Budget;

                    db.SaveChanges();
                }
            }
        }

        public void DeleteProject(int projectId)
        {
            using (FreelancerDbContext db = new FreelancerDbContext())
            {
                Project projectToDelete = db.Projects.Find(projectId);
                if (projectToDelete != null)
                {
                    db.Projects.Remove(projectToDelete);
                    db.SaveChanges();
                }
            }
        }
    }





}
