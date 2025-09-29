 
using Microsoft.AspNetCore.Identity;

namespace ERPManagement.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public ICollection<RoleBusinessObject?> RolesBusinessObjects { get; set; } = new List<RoleBusinessObject?>();
    }
}
