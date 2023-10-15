using FreelancerCLone.DbModels;
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

        public void AddProject(ProjectViewModel project)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            Project p = new Project();
            p.Deadline = project.Deadline;
            p.Description = project.Description;
            p.AddedOn = DateTime.Now;
            p.AddedBy = 1232;
            p.Id = project.Id;
            p.IsActive = false;
            p.Status = 1;
            p.TechnologyRequired ="fdhgnfv";
            p.Title = project.Title;
            p.Budget = project.Budget;
            db.Projects.Add(p);
            db.SaveChanges();
        }


        public List<ProjectViewModel> GetProjects()
        {
            List<ProjectViewModel> pro = new List<ProjectViewModel>();
            FreelancerDbContext db = new FreelancerDbContext();
            var projects = db.Projects.ToList();
            foreach (var p in projects)
            {
                ProjectViewModel viewModel = new ProjectViewModel();
                viewModel.Id = p.Id;
                viewModel.Title = p.Title;
                viewModel.Description  = p.Description;
                viewModel.Deadline = p.Deadline;
                viewModel.IsActive = p.IsActive;
                viewModel.Status = p.Status;
                viewModel.TechnologyRequired = p.TechnologyRequired;
                viewModel.Budget = p.Budget;
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
