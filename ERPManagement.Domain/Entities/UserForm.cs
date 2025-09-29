namespace ERPManagement.Domain.Entities
{
    public class UserForm
    {

        public int Id { get; set; }
        public bool? AddData { get; set; }
        public bool? UpdateData { get; set; }
        public bool? DeleteData { get; set; }
        public int UserId { get; set; }
        public int BusinessObjectsId { get; set; }
        public virtual User User { get; set; }
        public virtual BusinessObject BusinessObjects { get; set; }

    }
}
