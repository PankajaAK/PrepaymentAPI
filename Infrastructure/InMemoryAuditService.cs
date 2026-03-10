using PrepaymentAPI.Models;
using PrepaymentAPI.Services;

namespace PrepaymentAPI.Infrastructure
{
    public class InMemoryAuditService : IAuditService
    {
        private readonly List<AuditLog> _logs = new();
        private readonly ILogger<InMemoryAuditService> _logger;

        public InMemoryAuditService(
            ILogger<InMemoryAuditService> logger)
        {
            _logger = logger;
        }

        public Task LogAsync(AuditLog auditLog)
        {
            _logs.Add(auditLog);
            _logger.LogInformation(
                "Audit log recorded: Operation={Operation}, " +
                "DocumentId={DocumentId}, " +
                "Success={IsSuccess}, " +
                "Latency={LatencyMs}ms",
                auditLog.Operation,
                auditLog.DocumentId,
                auditLog.IsSuccess,
                auditLog.LatencyMs);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<AuditLog>> GetByDocumentIdAsync(
            string documentId)
        {
            var logs = _logs
                .Where(l => l.DocumentId == documentId)
                .OrderByDescending(l => l.Timestamp)
                .AsEnumerable();
            return Task.FromResult(logs);
        }
    }
}

