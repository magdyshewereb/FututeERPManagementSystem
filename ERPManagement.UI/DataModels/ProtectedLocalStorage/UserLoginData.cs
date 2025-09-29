namespace ERPManagement.UI.DataModels.ProtectedLocalStorage
{
    public class UserLoginData
    {
        public string DataBaseName { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string UserID { get; set; }
        public bool Remember { get; set; } = false;      
        public string BranchID { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }

    }
}
