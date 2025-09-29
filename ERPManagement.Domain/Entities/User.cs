using Microsoft.AspNetCore.Identity;

namespace ERPManagement.Domain.Entities;

public class User : IdentityUser<int>
{
    public bool? IsAdmin { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public int? StatusID { get; set; }
    public int? EmployeeId { get; set; }
    public string? WhatsAppNumber { get; set; }
    public string? ExtendedEntityInfo { get; set; } 
    public virtual ICollection<UserForm> UserForms { get; set; }
    public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; }

    public User()
    {
        UserForms = new HashSet<UserForm>();
        RefreshTokens = new HashSet<UserRefreshToken>();
    }


}
