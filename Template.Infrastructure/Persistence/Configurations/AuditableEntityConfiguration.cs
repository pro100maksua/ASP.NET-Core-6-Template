using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using $ext_safeprojectname$.Domain.Common;

namespace $ext_safeprojectname$.Infrastructure.Persistence.Configurations;

public class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedBy).HasMaxLength(250);
        builder.Property(e => e.LastModifiedBy).HasMaxLength(250);

        builder.Property(e => e.Created)
            .HasConversion(d => d, d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        builder.Property(e => e.LastModified)
            .HasConversion(d => d, d => DateTime.SpecifyKind(d.Value, DateTimeKind.Utc));
    }
}