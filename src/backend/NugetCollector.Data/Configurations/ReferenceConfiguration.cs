using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NugetCollector.Data.Model;

namespace NugetCollector.Data.Configurations
{
    internal class ReferenceConfiguration : IEntityTypeConfiguration<Reference>
    {
        public void Configure(EntityTypeBuilder<Reference> builder)
        {
            builder.HasKey(p => new { Id = p.Name, p.Version });
        }
    }
}