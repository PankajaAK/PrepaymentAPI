using PrepaymentAPI.Models;

namespace PrepaymentAPI.Services
{
    public interface IAuditService
    {
        Task LogAsync(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> GetByDocumentIdAsync(string documentId);
    }
}

