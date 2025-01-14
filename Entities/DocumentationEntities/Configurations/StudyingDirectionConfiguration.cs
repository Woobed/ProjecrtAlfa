using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectAlfa.Entities.DocumentationEntities.Configurations
{
    public class StudyingDirectionConfiguration:IEntityTypeConfiguration<StudyingDirection>
    {

        public void Configure(EntityTypeBuilder<StudyingDirection> builder) 
        { 
            builder.HasKey(sd=> sd.DirectionCode);
            builder.HasIndex(sd=>sd.DirectionCode).IsUnique();

            builder.Property(sd=>sd.DirectionName).HasMaxLength(45);
            builder.Property(sd=>sd.Level).HasMaxLength(45);

            builder.HasOne(sd=>sd.Group)
                .WithMany(bg => bg.StudyingDirections)
                .HasForeignKey(sd=>sd.GroupNumber);

            builder.HasMany(sd => sd.Profiles)
                .WithOne(p => p.Direction)
                .HasForeignKey(p => p.DirectionCode);

        }
    }
}
