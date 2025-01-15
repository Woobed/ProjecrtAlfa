using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProjectAlfa.Entities.DocumentationEntities.Configurations
{
    public class PrifileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p=>p.ProfileCode);
            builder.HasIndex(p => p.ProfileCode).IsUnique();
            
            builder.Property(p => p.ProfileName).HasMaxLength(45);
            
            builder.HasOne(p => p.Direction)
                .WithMany(sd => sd.Profiles)
                .HasForeignKey(p=>p.DirectionCode);

            builder.HasMany(p => p.Competencies)
                .WithOne(c => c.Profile);
        }
    }
}
