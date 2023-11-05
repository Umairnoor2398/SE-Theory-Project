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
		public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
		public virtual DbSet<FreelancerPersonalProject> FreelancerPersonalProjects { get; set; } = null!;
		public virtual DbSet<FreelancerPersonalProjectImage> FreelancerPersonalProjectImages { get; set; } = null!;
		public virtual DbSet<Log> Logs { get; set; } = null!;
		public virtual DbSet<Lookup> Lookups { get; set; } = null!;
		public virtual DbSet<Project> Projects { get; set; } = null!;
		public virtual DbSet<ProjectBid> ProjectBids { get; set; } = null!;
		public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; } = null!;
		public virtual DbSet<Skill> Skills { get; set; } = null!;
		public virtual DbSet<SkillCategory> SkillCategories { get; set; } = null!;
		public virtual DbSet<User> Users { get; set; } = null!;
		public virtual DbSet<UserSkill> UserSkills { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
			optionsBuilder.UseSqlServer("Server=(local); Database=FreelancerDb; Integrated Security=True; TrustServerCertificate=True");
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

			modelBuilder.Entity<FreelancerPersonalProject>(entity =>
			{
				entity.ToTable("FreelancerPersonalProject");

				entity.Property(e => e.Id).ValueGeneratedNever();

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

			modelBuilder.Entity<Log>(entity =>
			{
				entity.ToTable("Log");

				entity.Property(e => e.AddedOn).HasColumnType("datetime");

				entity.Property(e => e.Class).HasMaxLength(50);

				entity.Property(e => e.Function).HasMaxLength(50);

				entity.Property(e => e.Level).HasMaxLength(20);
			});

			modelBuilder.Entity<Lookup>(entity =>
			{
				entity.ToTable("Lookup");

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

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
