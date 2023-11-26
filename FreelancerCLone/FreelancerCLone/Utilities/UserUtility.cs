using FreelancerCLone.Constants;
using FreelancerCLone.DbModels;
using FreelancerCLone.Services;
using FreelancerCLone.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelancerCLone.Utilities
{
    // Utility class for managing user operations in the FreelancerClone application
    public class UserUtility
    {
        // Singleton pattern: Ensures only one instance of the UserUtility is created
        private static UserUtility _instance;

        public static UserUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserUtility();
                return _instance;
            }
        }

        //Retrieve the user ID based on the provided username
        public int GetUserId(string username)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var _user = db.AspNetUsers.Where(x => x.UserName == username).FirstOrDefault();
            return _user.Users.FirstOrDefault().Id;
        }

        //Retrieve the profile picture URL for the given username
        public string getUserProfilePictureUrl(string username)
        {
            int userId = GetUserId(username);
            FreelancerDbContext db = new FreelancerDbContext();
            return db.Users.Find(userId).ProfileImagePath;
        }

        //Add a new user to the database
        public void AddUser(UserViewModel user)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            User u = new User();
            u.FirstName = user.FirstName;
            u.Id = user.Id;
            u.LastName = user.LastName;
            u.AddedOn = DateTime.Now;
            u.UserId = user.UserId;
            u.ShortDescription = user.ShortDescription;
            u.LongDescription = user.LongDescription;
            u.IsActive = user.IsActive;
            u.ProfileImagePath = user.ProfileImagePath;
            u.Status = user.Status;
            db.Users.Add(u);
            db.SaveChanges();
        }

        //Retrieve user information for a user profile
        public User GetUserForProfile(int user, string username)
        {
            User userDb;
            FreelancerDbContext db = new FreelancerDbContext();
            if (user == 0)
            {
                int userId = UserUtility.Instance.GetUserId(username);
                userDb = db.Users.Find(userId);
            }
            else
            {
                userDb = db.Users.Find(user);
            }

            return userDb;
        }

        //Retrieve user information for a view model
        public UserViewModel GetUserViewModl(string username)
        {
            User userDb;
            FreelancerDbContext db = new FreelancerDbContext();
            int userId = UserUtility.Instance.GetUserId(username);
            userDb = db.Users.Find(userId);

            UserViewModel user = new UserViewModel();
            // Populate the UserViewModel with data from the database user entity
            user.FirstName = userDb.FirstName;
            user.LastName = userDb.LastName;
            user.ShortDescription = userDb.ShortDescription;
            user.LongDescription = userDb.LongDescription;
            user.ProfileImagePath = userDb.ProfileImagePath;

            return user;
        }

        //update user profile information
        public async Task UpdateProfileAsync(UserViewModel model, string username, IWebHostEnvironment _webHost)
        {
            var user = GetUserForProfile(0, username);
            FreelancerDbContext db = new FreelancerDbContext();
            // Update user information based on the provided model
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.ShortDescription = model.ShortDescription;
            user.LongDescription = model.LongDescription;
            // Prepare a list of file paths for user images
            List<FilePathEnum> path = new List<FilePathEnum>();

            path.Add(FilePathEnum.UserImages);
            user.ProfileImagePath = await UploadFileService.Instance.UploadFile(model.profileImage, path, _webHost);

            db.Users.Update(user);
            db.SaveChanges();
        }

        //Retrieve personal projects for a freelancer
        public List<FreelancerPersonalProject> GetFreelancerPersonalProjects(int user, string username)
        {
            List<FreelancerPersonalProject> freelancerProjects;
            FreelancerDbContext db = new FreelancerDbContext();

            int userId = user;

            if (user == 0)
            {
                userId = UserUtility.Instance.GetUserId(username);
            }
            freelancerProjects = db.FreelancerPersonalProjects.Where(x => x.UserId == userId && x.IsActive == true).ToList();
            return freelancerProjects;
        }

        // Retrieve a view model for a freelancer's personal project
        public FreelancerPersonalProjectViewModel GetFreelancerPersonalProjectViewModel(int id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var project = db.FreelancerPersonalProjects.Find(id);

            FreelancerPersonalProjectViewModel viewModel = new FreelancerPersonalProjectViewModel();
            viewModel.Id = project.Id;
            viewModel.Title = project.Title;
            viewModel.Description = project.Description;
            viewModel.StartDate = project.StartDate;
            viewModel.EndDate = project.EndDate;
            viewModel.Technology = project.Technology;
            viewModel.PublicUrl = project.PublicUrl;
            viewModel.UserId = project.UserId;
            viewModel.AddedOn = project.AddedOn;
            viewModel.UpdatedOn = project.UpdatedOn;
            viewModel.IsActive = project.IsActive;
            viewModel.FreelancerPersonalProjectImages = project.FreelancerPersonalProjectImages;
            return viewModel;
        }

        //Add a new freelancer personal project
        public async Task AddFreelancerPersonalProject(FreelancerPersonalProjectViewModel model, string username, IWebHostEnvironment _webHost)
        {
            // Create a new FreelancerPersonalProject entity and populate it with data from the provided model
            FreelancerDbContext db = new FreelancerDbContext();
            FreelancerPersonalProject proj = new FreelancerPersonalProject();
            proj.Title = model.Title;
            proj.Description = model.Description;
            proj.StartDate = model.StartDate;
            proj.EndDate = model.EndDate;
            proj.Technology = model.Technology;
            proj.PublicUrl = model.PublicUrl;
            proj.AddedOn = DateTime.Now;
            proj.UpdatedOn = DateTime.Now;
            proj.IsActive = true;
            // Retrieve the user ID based on the provided username and associate it with the project
            proj.UserId = UserUtility.Instance.GetUserId(username);

            db.FreelancerPersonalProjects.Add(proj);
            db.SaveChanges();

            List<FilePathEnum> path = new List<FilePathEnum>();

            path.Add(FilePathEnum.ProjectImages);
            // Iterate over each image in the model and associate it with the project
            foreach (var i in model.images)
            {
                FreelancerPersonalProjectImage img = new FreelancerPersonalProjectImage();
                img.PersonalProjectId = proj.Id;
                img.ImagePath = await UploadFileService.Instance.UploadFile(i, path, _webHost);
                img.AddedOn = DateTime.Now;
                img.UpdatedOn = DateTime.Now;
                img.IsActive = true;
                db.FreelancerPersonalProjectImages.Add(img);
            }

            db.SaveChanges();
        }

        // Edit an existing freelancer personal project
        public async Task EditFreelancerPersonalProject(FreelancerPersonalProjectViewModel model, string username, IWebHostEnvironment _webHost)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            var proj = db.FreelancerPersonalProjects.Find(model.Id);
            proj.Title = model.Title;
            proj.Description = model.Description;
            proj.StartDate = model.StartDate;
            proj.EndDate = model.EndDate;
            proj.Technology = model.Technology;
            proj.PublicUrl = model.PublicUrl;
            proj.UpdatedOn = DateTime.Now;

            db.FreelancerPersonalProjects.Update(proj);
            db.SaveChanges();

            List<FilePathEnum> path = new List<FilePathEnum>();

            path.Add(FilePathEnum.ProjectImages);
            // Iterate over each image in the model and associate it with the project

            foreach (var i in model.images)
            {
                FreelancerPersonalProjectImage img = new FreelancerPersonalProjectImage();
                img.PersonalProjectId = proj.Id;
                img.ImagePath = await UploadFileService.Instance.UploadFile(i, path, _webHost);
                // Set timestamps and activation status for the image
                img.AddedOn = DateTime.Now;
                img.UpdatedOn = DateTime.Now;
                img.IsActive = true;
                db.FreelancerPersonalProjectImages.Add(img);
            }

            db.SaveChanges();
        }

        //Retrieve skills for a user
        public List<UserSkill> GetUserSkills(int user, string username)
        {
            List<UserSkill> userSkills;
            FreelancerDbContext db = new FreelancerDbContext();

            if (user == 0)
            {
                user = UserUtility.Instance.GetUserId(username);
            }

            userSkills = db.UserSkills.Where(x => x.UserId == user && x.IsActive == true).ToList();
            return userSkills;
        }

        // AddUserSkill: Add a new skill for a user
        public void AddUserSkill(UserSkill model)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            db.UserSkills.Add(model);
            db.SaveChanges();
        }

        // Deactivate a user skill
        public void DeleteUserSkill(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();

            var skill = db.UserSkills.Find(Id);
            skill.IsActive = false;
            db.UserSkills.Update(skill);
            db.SaveChanges();
        }

        //Deactivate a user's personal projects
        public void DeleteUserPersonalProjects(int Id)
        {
            FreelancerDbContext db = new FreelancerDbContext();

            var project = db.FreelancerPersonalProjects.Find(Id);
            project.IsActive = false;
            db.FreelancerPersonalProjects.Update(project);
            db.SaveChanges();
        }

        public FreelancerPersonalProject GetFreelancerPersonalProject(int id)
        {
            FreelancerDbContext db = new FreelancerDbContext();
            return db.FreelancerPersonalProjects.Find(id);
        }
    }
}
