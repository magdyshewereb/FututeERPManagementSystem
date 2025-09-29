namespace ERPManagement.Domain.Entities
{
    public class UserLog
    {
        public int Id { get; set; } // Primary Key
        public int UserId { get; set; } // ID of the user who performed the action
        public string? Username { get; set; }           // New
        public string? IPAddress { get; set; }          // New
        public string? DeviceInfo { get; set; }         // New
        public string? Type { get; set; } // Action Type: Insert, Update, Delete
        public string? TableName { get; set; } // Name of the affected table
        public DateTime DateTime { get; set; } = DateTime.UtcNow; // Timestamp
        public string? OldValues { get; set; } // JSON: Original values before the change
        public string? NewValues { get; set; } // JSON: New values after the change
        public string? AffectedColumns { get; set; } // Comma-separated list of modified columns
        public string? PrimaryKey { get; set; } // Primary key of the affected record
    }


}

