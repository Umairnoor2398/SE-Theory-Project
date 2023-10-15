using iTextSharp.text.pdf.parser.clipper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FreelancerCLone.Constants;
using FreelancerCLone.DbModels;

namespace FreelancerCLone.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(RoleType.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleType.User.ToString()));

            User userProfile = new User();
            FreelancerDbContext db = new FreelancerDbContext();



            IUserStore<IdentityUser> _userStore = new UserStore<IdentityUser>(db);

            var user = Activator.CreateInstance<IdentityUser>();




            user.EmailConfirmed = true;
            user.Email = "admin@admin.com";
            user.UserName = "admin@admin.com";
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                var result = await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, RoleType.Admin.ToString());
                string userId = db.AspNetUsers.Where(x => x.Email == user.Email && x.PasswordHash == user.PasswordHash).FirstOrDefault().Id;
                userProfile.UserId = userId;
                userProfile.FirstName = "Bisma";
                userProfile.LastName = "Ali";
                userProfile.AddedOn = DateTime.Today.Date;
                userProfile.Status = db.Lookups.Where(x=>x.Value == "Accepted").FirstOrDefault().Id;
                userProfile.IsActive = true;
               
                db.Users.Add(userProfile);
                await db.SaveChangesAsync();
            }
        }
    }
}
