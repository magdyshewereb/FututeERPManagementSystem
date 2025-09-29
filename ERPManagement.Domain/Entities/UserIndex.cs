using ERPManagement.Domain.Common;

namespace ERPManagement.Domain.Entities
{
    public class UserIndex : AuditableEntity
    {
        public int UserId { get; set; }
        public int PageId { get; set; }
        public virtual User User { get; set; }
        public string? VisibleColumns { get; set; }

    }
}
