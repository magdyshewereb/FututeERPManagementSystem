namespace ERPManagement.Domain.Entities
{
    public class RoleBusinessObject
    {
        public int Id { get; set; }
        public int businessObjectId { get; set; }
        public int RoleId { get; set; }
        public virtual BusinessObject BusinessObject { get; set; }
        public virtual Role Role { get; set; }

    }
}
