using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NugetCollector.Data.Model;

namespace NugetCollector.Data.Configurations
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(p => p.ProjectReferences)
                .WithOne(p => p.Project)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}