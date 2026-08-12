namespace YourNamespace.GDPR.Entities
{
    public class LogEntry 
    {
        public Guid Id { get; set; }
        public DateTime LogDateTime { get; set; }
        public string Action { get; set; } // Ex: "UPDATE", "DELETE"
        public string EntityType { get; set; } 
        
        

        public LogEntry(string action, string entityType)
        {
            if (string.IsNullOrWhiteSpace(action)) throw new ArgumentException("Action cannot be null or empty", nameof(action));
            if (string.IsNullOrWhiteSpace(entityType)) throw new ArgumentException("EntityType cannot be null or empty", nameof(entityType));
            Id = Guid.NewGuid();
            LogDateTime = DateTime.UtcNow;
            Action = action;
            EntityType = entityType;
        }
    }
}