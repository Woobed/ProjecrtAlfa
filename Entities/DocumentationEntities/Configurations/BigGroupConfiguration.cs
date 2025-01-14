using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectAlfa.Entities.DocumentationEntities.Configurations
{
    public class BigGroupConfiguration :IEntityTypeConfiguration<BigGroup>
    {

        public void Configure(EntityTypeBuilder<BigGroup> builder)
        {
            builder.HasKey(bg => bg.GroupNumber);
            builder.HasIndex(bg => bg.GroupNumber).IsUnique();
            builder.Property(bg => bg.Name).HasMaxLength(45);
            builder.HasMany(bg => bg.StudyingDirections)
                .WithOne(sd => sd.Group)
                .HasForeignKey(sd => sd.GroupNumber);
        }
    }
}
