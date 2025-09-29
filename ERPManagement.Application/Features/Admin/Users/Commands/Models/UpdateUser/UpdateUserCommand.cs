using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Domain.Common;

namespace ERPManagement.Application.Features.Admin.Users.Commands.Models.UpdateUser
{
    public class UpdateUserCommand : AuditableEntity, ICommand<IMessage>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? IsAdmin { get; set; }
        public int? PageId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Email { get; set; }
        public int? ParentID { get; set; }
        public List<int> RolesId { get; set; }
        public virtual ICollection<BusinessObjectsVmQueryResult> UserForms { get; set; }

    }
}
