using ERPManagement.Domain.Common;

namespace ERPManagement.Domain.Entities
{
    public partial class SystemDictionary : AuditableEntity
    {
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public byte? ControlTypeId { get; set; }
        public int? SystemId { get; set; }
        public bool? ShowToUser { get; set; }
    }
}
