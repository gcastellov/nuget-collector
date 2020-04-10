using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NugetCollector.Data.Model;

namespace NugetCollector.Data.Configurations
{
    internal class ProjectReferencesConfiguration : IEntityTypeConfiguration<ProjectReference>
    {
        public void Configure(EntityTypeBuilder<ProjectReference> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}