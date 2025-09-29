namespace ERPManagement.Domain.Common
{
    public class AuditableEntity : IAuditableEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false; // Default to not deleted
        public DateTime? DeletedAt { get; set; } // Optional: Track when the deletion occurred
        public int? DeletedUser { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }



    }
}
