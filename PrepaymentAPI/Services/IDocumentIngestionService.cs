using PrepaymentAPI.Models;

namespace PrepaymentAPI.Services
{
    public interface IDocumentIngestionService
    {
        Task<string> IngestAsync(DocumentIngestionRequest request);
    }
}

