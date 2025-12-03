using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using class_library_disc.Models.Sql;

namespace class_library_disc.Data;

public partial class DiscProfileDbContext : DbContext
{
    public DiscProfileDbContext()
    {
    }

    public DiscProfileDbContext(DbContextOptions<DiscProfileDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompletionInterval> CompletionIntervals { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DiscProfile> DiscProfiles { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePrivateDatum> EmployeePrivateData { get; set; }

    public virtual DbSet<EmployeesOwnProfile> EmployeesOwnProfiles { get; set; }

    public virtual DbSet<EmployeesProject> EmployeesProjects { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectTask> ProjectTasks { get; set; }

    public virtual DbSet<ProjectTasksEmployee> ProjectTasksEmployees { get; set; }

    public virtual DbSet<ProjectsAudit> ProjectsAudits { get; set; }

    public virtual DbSet<ProjectsDiscProfile> ProjectsDiscProfiles { get; set; }

    public virtual DbSet<StressMeasure> StressMeasures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__company__3213E83FBC214E0F");

            entity.ToTable("company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BusinessField)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("business_field");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CompletionInterval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__completi__3213E83F8D36C5A8");

            entity.ToTable("completion_intervals");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TimeToComplete)
                .HasMaxLength(50)
                .HasColumnName("time_to_complete");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__departme__3213E83F7F71CF1A");

            entity.ToTable("departments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DiscProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__disc_pro__3213E83F671F06F3");

            entity.ToTable("disc_profiles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("color");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F3E48FF04");

            entity.ToTable("employees");

            entity.HasIndex(e => e.DepartmentId, "IX_employees_department_id");

            entity.HasIndex(e => e.DiscProfileId, "IX_employees_discProfile_id");

            entity.HasIndex(e => e.FirstName, "IX_employees_first_name");

            entity.HasIndex(e => e.LastName, "IX_employees_last_name");

            entity.HasIndex(e => e.WorkPhone, "IX_employees_phone");

            entity.HasIndex(e => e.PositionId, "IX_employees_position_id");

            entity.HasIndex(e => e.WorkEmail, "UQ__employee__0DD4ED79C16104C5").IsUnique();

            entity.HasIndex(e => e.WorkPhone, "UQ__employee__67295B202D9BC11D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DiscProfileId).HasColumnName("disc_profile_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_path");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.WorkEmail)
                .HasMaxLength(255)
                .HasColumnName("work_email");
            entity.Property(e => e.WorkPhone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("work_phone");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__employees__depar__505BE5AD");

            entity.HasOne(d => d.DiscProfile).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DiscProfileId)
                .HasConstraintName("FK__employees__disc___52442E1F");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__employees__posit__515009E6");
        });

        modelBuilder.Entity<EmployeePrivateDatum>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA83C6B2368");

            entity.ToTable("employee_private_data");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id");
            entity.Property(e => e.Cpr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cpr");
            entity.Property(e => e.PrivateEmail)
                .HasMaxLength(255)
                .HasColumnName("private_email");
            entity.Property(e => e.PrivatePhone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("private_phone");

            entity.HasOne(d => d.Employee).WithOne(p => p.EmployeePrivateDatum)
                .HasForeignKey<EmployeePrivateDatum>(d => d.EmployeeId)
                .HasConstraintName("FK__employee___emplo__55209ACA");
        });

        modelBuilder.Entity<EmployeesOwnProfile>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("employees_own_profile");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .HasColumnName("department_name");
            entity.Property(e => e.DiscProfileColor)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("disc_profile_color");
            entity.Property(e => e.DiscProfileName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("disc_profile_name");
            entity.Property(e => e.FullName)
                .HasMaxLength(511)
                .HasColumnName("full_name");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_path");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("position_name");
            entity.Property(e => e.PrivateEmail)
                .HasMaxLength(255)
                .HasColumnName("private_email");
            entity.Property(e => e.PrivatePhone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("private_phone");
            entity.Property(e => e.Username)
                .HasMaxLength(64)
                .HasColumnName("username");
            entity.Property(e => e.WorkEmail)
                .HasMaxLength(255)
                .HasColumnName("work_email");
            entity.Property(e => e.WorkPhone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("work_phone");
        });

        modelBuilder.Entity<EmployeesProject>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.EmployeeId }).HasName("PK__employee__202B7EA5F693CBBE");

            entity.ToTable("employees_projects");

            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.CurrentlyWorkingOn).HasColumnName("currently_working_on");
            entity.Property(e => e.IsProjectManager).HasColumnName("is_project_manager");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeesProjects)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__employees__emplo__70C8B53F");

            entity.HasOne(d => d.Project).WithMany(p => p.EmployeesProjects)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__employees__proje__6FD49106");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__position__3213E83F88B6D11C");

            entity.ToTable("positions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83F04E211B9");

            entity.ToTable("projects", tb => tb.HasTrigger("add_to_project_audit_table"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Deadline)
                .HasColumnType("datetime")
                .HasColumnName("deadline");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EmployeesNeeded).HasColumnName("employees_needed");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__project___3213E83F4F74A9C7");

            entity.ToTable("project_tasks", tb => tb.HasTrigger("trg_task_completion"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.TimeOfCompletion)
                .HasColumnType("datetime")
                .HasColumnName("time_of_completion");
            entity.Property(e => e.TimeToCompleteId).HasColumnName("time_to_complete_id");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectTasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__project_t__proje__6462DE5A");

            entity.HasOne(d => d.TimeToComplete).WithMany(p => p.ProjectTasks)
                .HasForeignKey(d => d.TimeToCompleteId)
                .HasConstraintName("FK__project_t__time___65570293");
        });

        modelBuilder.Entity<ProjectTasksEmployee>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.EmployeeId }).HasName("PK__project___98C0F43728C4ADD8");

            entity.ToTable("project_tasks_employees");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.CurrentlyWorkingOn).HasColumnName("currently_working_on");

            entity.HasOne(d => d.Employee).WithMany(p => p.ProjectTasksEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__project_t__emplo__6CF8245B");

            entity.HasOne(d => d.Task).WithMany(p => p.ProjectTasksEmployees)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__project_t__task___6C040022");
        });

        modelBuilder.Entity<ProjectsAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83FB38E7C5A");

            entity.ToTable("projects_audit");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("(suser_name())")
                .HasColumnName("action_by");
            entity.Property(e => e.ActionType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("action_type");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<ProjectsDiscProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83FCF42BFC5");

            entity.ToTable("projects_disc_profiles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscProfileId).HasColumnName("disc_profile_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");

            entity.HasOne(d => d.DiscProfile).WithMany(p => p.ProjectsDiscProfiles)
                .HasForeignKey(d => d.DiscProfileId)
                .HasConstraintName("FK__projects___disc___74994623");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsDiscProfiles)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__projects___proje__73A521EA");
        });

        modelBuilder.Entity<StressMeasure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__stress_m__3213E83F886F722A");

            entity.ToTable("stress_measures");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Measure).HasColumnName("measure");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.StressMeasures)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__stress_me__emplo__68336F3E");

            entity.HasOne(d => d.Task).WithMany(p => p.StressMeasures)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__stress_me__task___69279377");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__users__C52E0BA828408414");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "IX_users_username");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC5723C23AD95").IsUnique();

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.RequiresReset).HasColumnName("requires_reset");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(64)
                .HasColumnName("username");

            entity.HasOne(d => d.Employee).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.EmployeeId)
                .HasConstraintName("FK__users__employee___58F12BAE");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__user_role__59E54FE7");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_rol__3213E83F46351C99");

            entity.ToTable("user_roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
