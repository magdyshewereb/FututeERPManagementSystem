using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Contracts.Persistence;
using ERPManagement.Application.Shared.Enums;
using ERPManagement.Domain.Common;
using ERPManagement.Domain.Entities;
using ERPManagement.Persistence.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace ERPManagement.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfwork
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public UnitOfWork(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            _context.ChangeTracker.StateChanged += TrackChanges;
            _context.ChangeTracker.Tracked += TrackChanges;

        }

        public async Task<int> SaveAsync()
        {
            BeforeSaveChanges();
            return await _context.SaveChangesAsync();
        }

        public int Save()
        {
            BeforeSaveChanges();
            return _context.SaveChanges();
        }

        public async Task DisposeAsync() => await _context.DisposeAsync();

        public void Dispose() => _context.Dispose();

        private void TrackChanges(object sender, EntityEntryEventArgs e)
        {
            if (e.Entry.Entity is AuditableEntity entity &&
                e.Entry.Entity is not SystemDictionary &&
                e.Entry.Entity is not BusinessObject)
            {
                // Optional future logic
            }
        }

        private void BeforeSaveChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var auditEntries = new List<UserEntry>();

            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.Entity is UserLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                if (entry.Entity is not AuditableEntity auditable ||
                    entry.Entity is SystemDictionary ||
                    entry.Entity is BusinessObject)
                    continue;

                var auditEntry = new UserEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name
                };

                SetDefaultAuditFields(entry, auditable, auditEntry);

                auditEntries.Add(auditEntry);
            }

            foreach (var auditEntry in auditEntries)
            {
                _context.UserLogs.Add(auditEntry.ToAudit());
            }
        }

        private void SetDefaultAuditFields(EntityEntry entry, AuditableEntity auditable, UserEntry auditEntry)
        {
            var userId = _currentUserService.GetUserId();
            var ipAddress = _currentUserService.GetIpAddress();
            var getDeviceInfo = _currentUserService.GetDeviceInfo();
            var Username = _currentUserService.GetUserName();
            var now = DateTime.UtcNow;

            foreach (var prop in entry.Properties)
            {
                var propName = prop.Metadata.Name;

                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propName] = prop.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.CreationDate = now;
                        auditable.CreatedBy = userId;
                        auditEntry.LogType = LogType.Create;
                        auditEntry.NewValues[propName] = prop.CurrentValue;
                        auditEntry.UserId = userId;
                        Username = _currentUserService.GetUserName();
                        auditEntry.IPAddress = ipAddress;
                        auditEntry.DeviceInfo = getDeviceInfo;
                        break;

                    case EntityState.Modified:
                        if (prop.IsModified)
                        {
                            auditable.ModificationDate = now;
                            auditable.ModifiedBy = userId;
                            auditEntry.LogType = LogType.Update;
                            auditEntry.ChangedColumns.Add(propName);
                            auditEntry.OldValues[propName] = prop.OriginalValue;
                            auditEntry.NewValues[propName] = prop.CurrentValue;
                            auditEntry.UserId = userId;
                            Username = _currentUserService.GetUserName();
                            auditEntry.IPAddress = ipAddress;
                            auditEntry.DeviceInfo = getDeviceInfo;
                        }
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        auditable.IsDeleted = true;
                        auditable.DeletedAt = now;
                        auditable.DeletedUser = userId;
                        auditEntry.LogType = LogType.Delete;
                        auditEntry.OldValues[propName] = prop.OriginalValue;
                        auditEntry.UserId = userId;
                        Username = _currentUserService.GetUserName();
                        auditEntry.IPAddress = ipAddress;
                        auditEntry.DeviceInfo = getDeviceInfo;
                        break;
                }
            }
        }
    }

}