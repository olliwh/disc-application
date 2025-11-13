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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DiscProfile> DiscProfiles { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePrivateData> EmployeePrivateData { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectTask> ProjectTasks { get; set; }

    public virtual DbSet<ProjectsDiscProfile> ProjectsDiscProfiles { get; set; }

    public virtual DbSet<StressMeasure> StressMeasures { get; set; }

    public virtual DbSet<TaskCompleteInterval> TaskCompleteIntervals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database= disc_profile_relational_db;User Id=sa;Password=Pass@word1;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__companie__3213E83F7EE520E8");

            entity.ToTable("companies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BusinessField)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("business_field");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__departme__3213E83FC28B1094");

            entity.ToTable("departments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Company).WithMany(p => p.Departments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__departmen__compa__267ABA7A");
        });

        modelBuilder.Entity<DiscProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__disc_pro__3213E83FFB7D292A");

            entity.ToTable("disc_profiles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(255)
                .IsUnicode(false)
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
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83FD06FE8FA");

            entity.ToTable("employees");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DiscProfileId).HasColumnName("disc_profile_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_path");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.WorkEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("work_email");
            entity.Property(e => e.WorkPhone)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("work_phone");

            entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__employees__compa__2F10007B");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__employees__depar__300424B4");

            entity.HasOne(d => d.DiscProfile).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DiscProfileId)
                .HasConstraintName("FK__employees__disc___31EC6D26");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__employees__posit__30F848ED");
        });

        modelBuilder.Entity<EmployeePrivateData>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA8CCF64C08");

            entity.ToTable("employee_private_data");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id");
            entity.Property(e => e.Cpr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cpr");
            entity.Property(e => e.PrivateEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("private_email");
            entity.Property(e => e.PrivatePhone)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("private_phone");

            entity.HasOne(d => d.Employee).WithOne(p => p.EmployeePrivateDatum)
                .HasForeignKey<EmployeePrivateData>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__employee___emplo__34C8D9D1");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__position__3213E83F613FFA66");

            entity.ToTable("positions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83FB4A51640");

            entity.ToTable("projects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Deadline)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deadline");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NumberOfEmployees).HasColumnName("number_of_employees");

            entity.HasMany(d => d.Employees).WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeesProject",
                    r => r.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__employees__emplo__4BAC3F29"),
                    l => l.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__employees__proje__4AB81AF0"),
                    j =>
                    {
                        j.HasKey("ProjectId", "EmployeeId").HasName("PK__employee__202B7EA573A8744E");
                        j.ToTable("employees_projects");
                        j.IndexerProperty<int>("ProjectId").HasColumnName("project_id");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
                    });
        });

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__project___3213E83F89CD98EE");

            entity.ToTable("project_tasks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Evaluation)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("evaluation");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.TimeOfCompletion)
                .HasColumnType("datetime")
                .HasColumnName("time_of_completion");
            entity.Property(e => e.TimeToComplete).HasColumnName("time_to_complete");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectTasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__project_t__proje__3F466844");

            entity.HasOne(d => d.TimeToCompleteNavigation).WithMany(p => p.ProjectTasks)
                .HasForeignKey(d => d.TimeToComplete)
                .HasConstraintName("FK__project_t__time___403A8C7D");

            entity.HasMany(d => d.Employees).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectTasksEmployee",
                    r => r.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__project_t__emplo__47DBAE45"),
                    l => l.HasOne<ProjectTask>().WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__project_t__task___46E78A0C"),
                    j =>
                    {
                        j.HasKey("TaskId", "EmployeeId").HasName("PK__project___98C0F4372EC7BB12");
                        j.ToTable("project_tasks_employees");
                        j.IndexerProperty<int>("TaskId").HasColumnName("task_id");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("employee_id");
                    });
        });

        modelBuilder.Entity<ProjectsDiscProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83F8523F5A4");

            entity.ToTable("projects_disc_profiles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscProfileId).HasColumnName("disc_profile_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");

            entity.HasOne(d => d.DiscProfile).WithMany(p => p.ProjectsDiscProfiles)
                .HasForeignKey(d => d.DiscProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projects___disc___4F7CD00D");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectsDiscProfiles)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projects___proje__4E88ABD4");
        });

        modelBuilder.Entity<StressMeasure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__stress_m__3213E83FD6F53084");

            entity.ToTable("stress_measures");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Measure).HasColumnName("measure");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.StressMeasures)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stress_me__emplo__4316F928");

            entity.HasOne(d => d.Task).WithMany(p => p.StressMeasures)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stress_me__task___440B1D61");
        });

        modelBuilder.Entity<TaskCompleteInterval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__task_com__3213E83FA1C9A8DD");

            entity.ToTable("task_complete_intervals");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TimeToComplete)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("time_to_complete");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__users__C52E0BA8E67AAAD1");

            entity.ToTable("users");

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
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Employee).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__employee___37A5467C");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__user_role__38996AB5");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_rol__3213E83F03340981");

            entity.ToTable("user_roles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
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
