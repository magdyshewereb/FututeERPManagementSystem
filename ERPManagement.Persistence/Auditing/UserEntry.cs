using ERPManagement.Application.Shared.Enums;
using ERPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace ERPManagement.Persistence.Auditing
{
    public class UserEntry(EntityEntry entry)
    {
        public EntityEntry Entry { get; } = entry;
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? IPAddress { get; set; }
        public string? DeviceInfo { get; set; }
        public string? TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new();
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new();
        public LogType LogType { get; set; }
        public List<string> ChangedColumns { get; } = new();

        public UserLog ToAudit()
        {
            return new UserLog
            {
                UserId = UserId,
                Username = Username,
                IPAddress = IPAddress,
                DeviceInfo = DeviceInfo,
                Type = LogType.ToString(),
                TableName = TableName,
                DateTime = DateTime.UtcNow,
                PrimaryKey = Serialize(KeyValues),
                OldValues = SerializeOrNull(OldValues),
                NewValues = SerializeOrNull(NewValues),
                AffectedColumns = SerializeOrNull(ChangedColumns)
            };

        }

        private string SerializeOrNull<T>(ICollection<T> collection)
        {
            return collection == null || collection.Count == 0
                ? "null"
                : JsonConvert.SerializeObject(collection, Formatting.None);
        }
        private string Serialize(Dictionary<string, object> dict)
        {
            return dict.Count == 0 ? "null" : JsonConvert.SerializeObject(dict, Formatting.None);
        }
    }
}
