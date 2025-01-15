using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectAlfa.Entities.DocumentationEntities.Configurations
{
    public class CompetencyConfiguration : IEntityTypeConfiguration<Competency>
    {
        public void Configure(EntityTypeBuilder<Competency> builder)
        {
            builder.HasKey(c => new { c.CompetencyCode, c.ProfileCode });
            builder.HasIndex(c => new { c.CompetencyCode, c.ProfileCode }).IsUnique();

            builder.Property(c => c.Wording).HasMaxLength(45);
            builder.Property(c => c.CompetencyCipher).HasMaxLength(20);
            builder.Property(c => c.CompetencyType).HasMaxLength(45);

        }

    }
}
