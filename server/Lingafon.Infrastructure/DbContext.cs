using Lingafon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure;

public class LingafonDbContext : DbContext
{
    public LingafonDbContext(DbContextOptions<LingafonDbContext> options) : base(options)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entityType.FindProperty("Id");
            if (idProperty != null && idProperty.ClrType == typeof(Guid))
            {
                idProperty.SetDefaultValueSql("gen_random_uuid()");
            }
        }
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentResult> AssignmentResults { get; set; }
    public DbSet<Dialog> Dialogs { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<InviteLink> InviteLinks { get; set; }
    public DbSet<TeacherStudent>  TeacherStudents { get; set; }
}