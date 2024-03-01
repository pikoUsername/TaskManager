using Microsoft.EntityFrameworkCore;
using TaskManager.Database.Models; 

namespace TaskManager.Database; 

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<TaskTag> TaskTags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<DayTimetable> DayTimetables { get; set; }
    public DbSet<WorkVisit> WorkVisits { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }
    public DbSet<FileModel> FileModels { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
} 