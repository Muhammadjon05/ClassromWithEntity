using IdentityData.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityData.AppDbContext;

public class Context : IdentityDbContext<User,Role,Guid>
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
        builder.Entity<User>().ToTable("users");
        builder.Entity<User>().Property(i => i.FirstName).HasColumnName("firstname").HasMaxLength(19).IsRequired();
        builder.Entity<User>().Property(i => i.LastName).HasColumnName("lastname").HasMaxLength(19).IsRequired(false);
        builder.Entity<User>().Property(i => i.PhotoUrl).HasColumnName("photo_url").HasMaxLength(1000).IsRequired(false);
        builder.Entity<UserTask>().HasKey(t => new { t.UserId, t.TaskId });
    }
    public DbSet<School> Schools { get; set; }
    public DbSet<UserSchool> UserSchools { get; set; }
    public DbSet<Science> Sciences { get; set; }
    public DbSet<UserScience> UserSciences { get; set; }
    public DbSet<SendRequestToJoin> SendRequestToJoins { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<TaskEntity> TaskEntities { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
}