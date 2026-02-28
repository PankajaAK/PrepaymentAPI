using Microsoft.AspNetCore.Mvc;
using PrepaymentAPI.Services;

namespace PrepaymentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditController> _logger;

        public AuditController(
            IAuditService auditService,
            ILogger<AuditController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        /// <summary>
        /// Get full audit trail for a document
        /// </summary>
        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetAuditTrail(string documentId)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving audit trail for document {DocumentId}",
                    documentId);

                var logs = await _auditService
                    .GetByDocumentIdAsync(documentId);

                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to retrieve audit trail for {DocumentId}",
                    documentId);

                return StatusCode(500,
                    new { Error = "Failed to retrieve audit trail",
                        Details = ex.Message });
            }
        }
    }
}
