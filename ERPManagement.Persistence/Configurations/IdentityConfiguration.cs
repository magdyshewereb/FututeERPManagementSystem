using ERPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPManagement.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ////////////////// Properties //////////////////

            builder.Property(e => e.UserName).HasMaxLength(256);
            builder.Property(e => e.NormalizedUserName).HasMaxLength(256);
            builder.Property(e => e.Email).HasMaxLength(256);
            builder.Property(e => e.NormalizedEmail).HasMaxLength(256);
            builder.Property(e => e.PasswordHash).HasMaxLength(256);
            builder.Property(e => e.SecurityStamp).HasMaxLength(256);
            builder.Property(e => e.ConcurrencyStamp).HasMaxLength(256);
            builder.Property(e => e.PhoneNumber).HasMaxLength(50); // اختياري، حسب احتياجك
            builder.ToTable("Users");

            ////////////////// Relationships //////////////////
            //// User <-> Employee

            //builder.HasOne(u => u.Employee)
            //       .WithMany(e => e.Users)
            //       .HasForeignKey(u => u.EmployeeId)
            //       .OnDelete(DeleteBehavior.SetNull);


        }
    }
    public class UserLogConfiguration : IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            ////////////////// Properties //////////////////

            builder.Property(e => e.OldValues).HasMaxLength(256);
            builder.Property(e => e.NewValues).HasMaxLength(256);
            builder.Property(e => e.AffectedColumns).HasMaxLength(256);

        }
    }
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(256);
            builder.Property(e => e.NormalizedName).HasMaxLength(256);
            builder.Property(e => e.ConcurrencyStamp).HasMaxLength(256);
            builder.ToTable("Roles");
        }
    }
    public class UserFormConfiguration : IEntityTypeConfiguration<UserForm>
    {
        public void Configure(EntityTypeBuilder<UserForm> builder)
        {

            builder.HasOne(uf => uf.User)
                   .WithMany(u => u.UserForms)
                   .HasForeignKey(uf => uf.UserId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(uf => uf.BusinessObjects)
                   .WithMany()
                   .HasForeignKey(uf => uf.BusinessObjectsId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class UserIndexConfiguration : IEntityTypeConfiguration<UserIndex>
    {
        public void Configure(EntityTypeBuilder<UserIndex> builder)
        {
            builder.HasOne(ui => ui.User)
                   .WithMany()
                   .HasForeignKey(ui => ui.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class RoleBusinessObjectConfiguration : IEntityTypeConfiguration<RoleBusinessObject>
    {
        public void Configure(EntityTypeBuilder<RoleBusinessObject> builder)
        {
            builder.HasOne(rbo => rbo.Role)
                   .WithMany(r => r.RolesBusinessObjects)
                   .HasForeignKey(rbo => rbo.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rbo => rbo.BusinessObject)
                   .WithMany()
                   .HasForeignKey(rbo => rbo.businessObjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {


            ////////////////// Relationships ////////////////// 

            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
