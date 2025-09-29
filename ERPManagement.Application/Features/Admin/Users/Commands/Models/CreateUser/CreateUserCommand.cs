using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Domain.Common;

namespace ERPManagement.Application.Features.Admin.Users.Commands.Models.CreateUser
{
    public class CreateUserCommand : AuditableEntity, ICommand<IMessage>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public bool? IsAdmin { get; set; }
        public List<int> RolesId { get; set; }
        public ICollection<BusinessObjectsVmQueryResult> UserForms { get; set; }
        public int? ParentID { get; set; }

    }

}
