using ERPManagement.Domain.Common;

namespace ERPManagement.Domain.Entities;

public partial class BusinessObject : AuditableEntity
{
    public string? Name { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public byte? Value { get; set; }
    public int? SystemId { get; set; }
    public int? Orders { get; set; }
    public bool? ShowToUser { get; set; }
    public int? ParentId { get; set; }
    public string? WebUrl { get; set; }
    public string? CssClassName { get; set; }
    public string? ActionName { get; set; }
    public string? ControllerName { get; set; }
    public string? RouteParmeters { get; set; }
    public int? AllowedPermissions { get; set; }
    public int? BusinessObjectID { get; set; }
    public BusinessObject? ParentObject { get; set; }
    public ICollection<BusinessObject> ChildObject { get; set; } = new List<BusinessObject>();
}