using ERPManagement.Domain.Common;

namespace ERPManagement.Application.Features.Common.Lockups.Queries.Models
{
    public class SystemLockupDetailVm : AuditableEntity
    {
        public int SystemLockupTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public bool? Active { get; set; }
        public bool? IsDefault { get; set; }
        public string? Note { get; set; }
        public int RowState { get; set; }
        public bool? IsReserved { get; set; }
        public string? ConfigValue { get; set; }
    }
}
