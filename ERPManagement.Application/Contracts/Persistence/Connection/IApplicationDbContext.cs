using ERPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace ERPManagement.Application.Contracts.Persistence.Connection
{
    public interface IApplicationDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);


        DbSet<BusinessObject> BusinessObjects { get; set; }
        DbSet<RoleBusinessObject> RolesBusinessObjects { get; set; }
        DbSet<SystemDictionary> SystemDictionary { get; set; }
        DbSet<UserForm> UserForms { get; set; }
        DbSet<UserLog> UserLogs { get; set; }
        DbSet<UserIndex> UsersIndex { get; set; }
        DbSet<UserRefreshToken> UserRefreshTokens { get; set; }



    }
}
