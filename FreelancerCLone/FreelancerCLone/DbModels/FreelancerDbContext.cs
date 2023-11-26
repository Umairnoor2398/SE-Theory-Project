using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FreelancerCLone.DbModels
{
    public partial class FreelancerDbContext : DbContext
    {
        public FreelancerDbContext()
        {
        }

        public FreelancerDbContext(DbContextOptions<FreelancerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<AspNetUsersAudit> AspNetUsersAudits { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<FeedbackAudit> FeedbackAudits { get; set; } = null!;
        public virtual DbSet<FreelancerPersonalProject> FreelancerPersonalProjects { get; set; } = null!;
        public virtual DbSet<FreelancerPersonalProjectAudit> FreelancerPersonalProjectAudits { get; set; } = null!;
        public virtual DbSet<FreelancerPersonalProjectImage> FreelancerPersonalProjectImages { get; set; } = null!;
        public virtual DbSet<FreelancerPersonalProjectImageAudit> FreelancerPersonalProjectImageAudits { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Lookup> Lookups { get; set; } = null!;
        public virtual DbSet<LookupAudit> LookupAudits { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectAudit> ProjectAudits { get; set; } = null!;
        public virtual DbSet<ProjectBid> ProjectBids { get; set; } = null!;
        public virtual DbSet<ProjectBidAudit> ProjectBidAudits { get; set; } = null!;
        public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; } = null!;
        public virtual DbSet<ProjectDocumentAudit> ProjectDocumentAudits { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<SkillAudit> SkillAudits { get; set; } = null!;
        public virtual DbSet<SkillCategory> SkillCategories { get; set; } = null!;
        public virtual DbSet<SkillCategoryAudit> SkillCategoryAudits { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserAudit> UserAudits { get; set; } = null!;
        public virtual DbSet<UserSkill> UserSkills { get; set; } = null!;
        public virtual DbSet<UserSkillAudit> UserSkillAudits { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local); Database=FreelancerDb; Integrated Security=True; TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.RoleId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsersAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__AspNetUs__A17F2398811E9E52");

                entity.ToTable("AspNetUsers_Audit", "Audit");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AddedByNavigation)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.AddedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_User");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.FeedbackCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Lookup");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.FeedbackStatusNavigations)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Lookup1");
            });

            modelBuilder.Entity<FeedbackAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Feedback__A17F239856817BC9");

                entity.ToTable("Feedback_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");
            });

            modelBuilder.Entity<FreelancerPersonalProject>(entity =>
            {
                entity.ToTable("FreelancerPersonalProject");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FreelancerPersonalProjects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FreelancerPersonalProject_User1");
            });

            modelBuilder.Entity<FreelancerPersonalProjectAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Freelanc__A17F2398F5A0DD74");

                entity.ToTable("FreelancerPersonalProject_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<FreelancerPersonalProjectImage>(entity =>
            {
                entity.ToTable("FreelancerPersonalProjectImage");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.PersonalProject)
                    .WithMany(p => p.FreelancerPersonalProjectImages)
                    .HasForeignKey(d => d.PersonalProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FreelancerPersonalProjectImage_FreelancerPersonalProject");
            });

            modelBuilder.Entity<FreelancerPersonalProjectImageAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Freelanc__A17F23980FFB784E");

                entity.ToTable("FreelancerPersonalProjectImage_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log");

                entity.Property(e => e.Level).HasMaxLength(255);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(255);

                entity.Property(e => e.MachineName).HasMaxLength(255);
            });

            modelBuilder.Entity<Lookup>(entity =>
            {
                entity.ToTable("Lookup");

                entity.Property(e => e.Category).HasMaxLength(100);

                entity.Property(e => e.Value).HasMaxLength(200);
            });

            modelBuilder.Entity<LookupAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Lookup_A__A17F2398EAC6E8FD");

                entity.ToTable("Lookup_Audit", "Audit");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Category).HasMaxLength(100);

                entity.Property(e => e.Value).HasMaxLength(200);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsAssigned).HasDefaultValueSql("((0))");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AddedByNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.AddedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_User");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Lookup");
            });

            modelBuilder.Entity<ProjectAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Project___A17F23980034627D");

                entity.ToTable("Project_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProjectBid>(entity =>
            {
                entity.ToTable("ProjectBid");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsCompleted)
                    .HasColumnName("isCompleted")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsReviewed)
                    .HasColumnName("isReviewed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rating).HasDefaultValueSql("((0.0))");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectBids)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectBid_Project");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.ProjectBids)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectBid_Lookup");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectBids)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectBid_User");
            });

            modelBuilder.Entity<ProjectBidAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__ProjectB__A17F2398488852BA");

                entity.ToTable("ProjectBid_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProjectDocument>(entity =>
            {
                entity.ToTable("ProjectDocument");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.DocumentType).HasMaxLength(10);

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectDocuments)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectDocument_Project");
            });

            modelBuilder.Entity<ProjectDocumentAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__ProjectD__A17F239839C996BD");

                entity.ToTable("ProjectDocument_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.DocumentType).HasMaxLength(10);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skill");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SkillName).HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.SkillCategory)
                    .WithMany(p => p.Skills)
                    .HasForeignKey(d => d.SkillCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Skill_SkillCategory");
            });

            modelBuilder.Entity<SkillAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Skill_Au__A17F2398713438C6");

                entity.ToTable("Skill_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.SkillName).HasMaxLength(200);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<SkillCategory>(entity =>
            {
                entity.ToTable("SkillCategory");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<SkillCategoryAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__SkillCat__A17F2398A54FA41D");

                entity.ToTable("SkillCategory_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.ShortDescription).HasMaxLength(1500);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Lookup");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_AspNetUsers");
            });

            modelBuilder.Entity<UserAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__User_Aud__A17F23980D5C078B");

                entity.ToTable("User_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.ShortDescription).HasMaxLength(1500);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(128);
            });

            modelBuilder.Entity<UserSkill>(entity =>
            {
                entity.ToTable("UserSkill");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.UserSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSkill_Skill");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSkills)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSkill_User");
            });

            modelBuilder.Entity<UserSkillAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__UserSkil__A17F2398E2A4DB63");

                entity.ToTable("UserSkill_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
