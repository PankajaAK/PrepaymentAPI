using Microsoft.AspNetCore.Mvc;
using PrepaymentAPI.Models;
using PrepaymentAPI.Services;

namespace PrepaymentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentIngestionService _ingestionService;
        private readonly IAuditService _auditService;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(
            IDocumentIngestionService ingestionService,
            IAuditService auditService,
            ILogger<DocumentController> logger)
        {
            _ingestionService = ingestionService;
            _auditService = auditService;
            _logger = logger;
        }

        /// <summary>
        /// Ingest a loan document for prepayment risk analysis
        /// </summary>
        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest(
            [FromBody] DocumentIngestionRequest request)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                _logger.LogInformation(
                    "Ingesting document {DocumentId} for borrower {BorrowerName}",
                    request.DocumentId,
                    request.BorrowerName);

                var documentId = await _ingestionService.IngestAsync(request);

                await _auditService.LogAsync(new AuditLog
                {
                    DocumentId = documentId,
                    Operation = "DocumentIngestion",
                    InputSummary = $"Borrower: {request.BorrowerName}, " +
                                  $"Loan: {request.LoanAmount}, " +
                                  $"Tenure: {request.TenureMonths} months",
                    OutputSummary = "Document ingested successfully",
                    LatencyMs = stopwatch.Elapsed.TotalMilliseconds,
                    IsSuccess = true
                });

                return Ok(new { DocumentId = documentId,
                    Message = "Document ingested successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to ingest document {DocumentId}",
                    request.DocumentId);

                await _auditService.LogAsync(new AuditLog
                {
                    DocumentId = request.DocumentId,
                    Operation = "DocumentIngestion",
                    InputSummary = $"Borrower: {request.BorrowerName}",
                    OutputSummary = "Ingestion failed",
                    LatencyMs = stopwatch.Elapsed.TotalMilliseconds,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });

                return StatusCode(500,
                    new { Error = "Document ingestion failed",
                        Details = ex.Message });
            }
        }
    }
}
 