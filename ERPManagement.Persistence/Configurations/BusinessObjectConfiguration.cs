using ERPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPManagement.Persistence.Configurations
{
    public class BusinessObjectConfiguration : IEntityTypeConfiguration<BusinessObject>
    {
        public void Configure(EntityTypeBuilder<BusinessObject> builder)
        {
            // BusinessObject: Self-reference (Parent/Child)

            builder.HasOne(b => b.ParentObject)
                   .WithMany(b => b.ChildObject)
                   .HasForeignKey(b => b.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
