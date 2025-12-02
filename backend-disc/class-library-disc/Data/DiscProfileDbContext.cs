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
            entity.HasKey(e => e.Id).HasName("PK__company__3213E83F58F8EDE6");

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
            entity.HasKey(e => e.Id).HasName("PK__completi__3213E83F6B3D5BAF");

            entity.ToTable("completion_intervals");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TimeToComplete)
                .HasMaxLength(50)
                .HasColumnName("time_to_complete");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__departme__3213E83FDC4A124C");

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
            entity.HasKey(e => e.Id).HasName("PK__disc_pro__3213E83F5581FE1A");

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
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F8B60120A");

            entity.ToTable("employees");

            entity.HasIndex(e => e.DepartmentId, "IX_employees_department_id");

            entity.HasIndex(e => e.DiscProfileId, "IX_employees_discProfile_id");

            entity.HasIndex(e => e.FirstName, "IX_employees_first_name");

            entity.HasIndex(e => e.LastName, "IX_employees_last_name");

            entity.HasIndex(e => e.WorkPhone, "IX_employees_phone");

            entity.HasIndex(e => e.PositionId, "IX_employees_position_id");

            entity.HasIndex(e => e.WorkEmail, "UQ__employee__0DD4ED79C2BCCA1D").IsUnique();

            entity.HasIndex(e => e.WorkPhone, "UQ__employee__67295B20DD3AEDCF").IsUnique();

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
                .HasConstraintName("FK__employees__depar__0D99FE17");

            entity.HasOne(d => d.DiscProfile).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DiscProfileId)
                .HasConstraintName("FK__employees__disc___0F824689");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__employees__posit__0E8E2250");
        });

        modelBuilder.Entity<EmployeePrivateDatum>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA85D4725C8");

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
                .HasConstraintName("FK__employee___emplo__125EB334");
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
            entity.HasKey(e => new { e.ProjectId, e.EmployeeId }).HasName("PK__employee__202B7EA5DC888D74");

            entity.ToTable("employees_projects");

            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.CurrentlyWorkingOn).HasColumnName("currently_working_on");
            entity.Property(e => e.IsProjectManager).HasColumnName("is_project_manager");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeesProjects)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__employees__emplo__2E06CDA9");

            entity.HasOne(d => d.Project).WithMany(p => p.EmployeesProjects)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__employees__proje__2D12A970");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__position__3213E83FCAF9BC9E");

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
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83F5397537B");

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
            entity.HasKey(e => e.Id).HasName("PK__project___3213E83F0315B56F");

            entity.ToTable("project_tasks", tb => tb.HasTrigger("trg_task_completion"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Evaluation)
                .HasMaxLength(255)
                .HasColumnName("evaluation");
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
                .HasConstraintName("FK__project_t__proje__21A0F6C4");

            entity.HasOne(d => d.TimeToComplete).WithMany(p => p.ProjectTasks)
                .HasForeignKey(d => d.TimeToCompleteId)
                .HasConstraintName("FK__project_t__time___22951AFD");
        });

        modelBuilder.Entity<ProjectTasksEmployee>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.EmployeeId }).HasName("PK__project___98C0F4379084294F");

            entity.ToTable("project_tasks_employees");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.CurrentlyWorkingOn).HasColumnName("currently_working_on");

            entity.HasOne(d => d.Employee).WithMany(p => p.ProjectTasksEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__project_t__emplo__2A363CC5");

            entity.HasOne(d => d.ProjectTask).WithMany(p => p.ProjectTasksEmployees)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__project_t__task___2942188C");
        });

        modelBuilder.Entity<ProjectsAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83F855B68D7");

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
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83FA415EA35");

            entity.ToTable("projects_disc_profiles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscProfileId).HasColumnName("disc_profile_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");

            entity.HasOne(d => d.DiscProfile).WithMany(p => p.ProjectsDiscProfiles)
                .HasForeignKey(d => d.DiscProfileId)
                .HasConstraintName("FK__projects___disc___31D75E8D");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsDiscProfiles)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__projects___proje__30E33A54");
        });

        modelBuilder.Entity<StressMeasure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__stress_m__3213E83F7E35CB64");

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
                .HasConstraintName("FK__stress_me__emplo__257187A8");

            entity.HasOne(d => d.Task).WithMany(p => p.StressMeasures)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__stress_me__task___2665ABE1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__users__C52E0BA8DC1174BA");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "IX_users_username");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572D358499A").IsUnique();

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
                .HasConstraintName("FK__users__employee___162F4418");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__user_role__17236851");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_rol__3213E83F223F1904");

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
