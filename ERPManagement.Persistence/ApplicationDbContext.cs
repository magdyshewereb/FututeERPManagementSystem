using ERPManagement.Application.Contracts.Persistence.Connection;
using ERPManagement.Application.Contracts.Services;
using ERPManagement.Domain.Common;
using ERPManagement.Domain.Entities;
using ERPManagement.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ERPManagement.Persistence
{
    public class ApplicationDbContext
        : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
          IdentityUserRole<int>, IdentityUserLogin<int>,
          IdentityRoleClaim<int>, IdentityUserToken<int>>
        , IApplicationDbContext
    {

        private readonly ILoggedInUserService? _loggedInUserService;
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);


        }
        public IDbConnection Connection => Database.GetDbConnection();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // هذا السطر مهم جداً

            //دفعة واحدة بدون الحاجة لكتابةConfiguration هو طريقة مختصرة لتسجيل جميع ملفات الـ 
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.GlobalConfigure();


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Check if the entity implements ISoftDeletable
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext).GetMethod(nameof(SetSoftDeleteFilter)
                        , System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                                  ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");


        }
        // Static helper method to set the query filter
        private static void SetSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDeletable
        {
            modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            //{
            //    switch (entry.State)
            //    {

            //        case entitiestate.Added:
            //            entry.Entity.CreatedDate = DateTime.Now;
            //            entry.Entity.CreatedBy = _loggedInUserService.UserId;
            //            break;
            //        case entitiestate.Modified:
            //            entry.Entity.LastModifiedDate = DateTime.Now;
            //            entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
            //            break;
            //    }
            //}
            return base.SaveChangesAsync(cancellationToken);
        }


        public DbSet<BusinessObject> BusinessObjects { get; set; }
        public DbSet<RoleBusinessObject> RolesBusinessObjects { get; set; }
        public DbSet<SystemDictionary> SystemDictionary { get; set; }
        public DbSet<UserForm> UserForms { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<UserIndex> UsersIndex { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
