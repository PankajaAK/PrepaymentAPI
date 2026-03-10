using PrepaymentAPI.Models;
using PrepaymentAPI.Services;

namespace PrepaymentAPI.Infrastructure
{
    public class StubDocumentIngestionService : IDocumentIngestionService
    {
        private readonly ILogger<StubDocumentIngestionService> _logger;

        public StubDocumentIngestionService(
            ILogger<StubDocumentIngestionService> logger)
        {
            _logger = logger;
        }

        public Task<string> IngestAsync(DocumentIngestionRequest request)
        {
            _logger.LogInformation(
                "Ingesting document for borrower {BorrowerName}",
                request.BorrowerName);

            // Stub implementation — real Azure Document Intelligence
            // and AI Search integration 
            var documentId = string.IsNullOrEmpty(request.DocumentId)
                ? Guid.NewGuid().ToString()
                : request.DocumentId;

            return Task.FromResult(documentId);
        }
    }
}

