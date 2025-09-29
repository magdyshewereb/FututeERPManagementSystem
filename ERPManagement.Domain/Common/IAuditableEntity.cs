namespace ERPManagement.Domain.Common
{
    public interface IAuditableEntity
    {
        int Id { get; set; }
        int? CreatedBy { get; set; }
        DateTime? CreationDate { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? ModificationDate { get; set; }
    }
}