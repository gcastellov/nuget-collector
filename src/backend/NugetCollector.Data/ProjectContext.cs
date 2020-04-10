using Microsoft.EntityFrameworkCore;
using NugetCollector.Data.Model;

namespace NugetCollector.Data
{
    public class ProjectContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Reference> References { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ReferenceConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ProjectReferencesConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Data Source=projects.db");
        }
    }
}