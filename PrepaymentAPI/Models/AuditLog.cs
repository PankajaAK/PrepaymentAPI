namespace PrepaymentAPI.Models
{
    public class AuditLog
    {
        public string AuditId { get; set; } = Guid.NewGuid().ToString();
        public string DocumentId { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string InputSummary { get; set; } = string.Empty;
        public string OutputSummary { get; set; } = string.Empty;
        public int TokensUsed { get; set; }
        public double LatencyMs { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
