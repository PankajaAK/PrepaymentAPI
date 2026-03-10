using Microsoft.AspNetCore.Mvc;
using PrepaymentAPI.Models;
using PrepaymentAPI.Services;

namespace PrepaymentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskController : ControllerBase
    {
        private readonly IRiskAnalysisService _riskService;
        private readonly IAuditService _auditService;
        private readonly ILogger<RiskController> _logger;

        public RiskController(
            IRiskAnalysisService riskService,
            IAuditService auditService,
            ILogger<RiskController> logger)
        {
            _riskService = riskService;
            _auditService = auditService;
            _logger = logger;
        }

        /// <summary>
        /// Analyze prepayment risk for an ingested document
        /// </summary>
        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze(
            [FromBody] RiskAnalysisRequest request)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                _logger.LogInformation(
                    "Analyzing risk for document {DocumentId}",
                    request.DocumentId);

                var response = await _riskService.AnalyzeAsync(request);

                await _auditService.LogAsync(new AuditLog
                {
                    DocumentId = request.DocumentId,
                    Operation = "RiskAnalysis",
                    InputSummary = $"Query: {request.Query}",
                    OutputSummary = $"Risk Level: {response.RiskLevel}, " +
                                   $"Confidence: {response.ConfidenceScore}",
                    TokensUsed = 0,
                    LatencyMs = stopwatch.Elapsed.TotalMilliseconds,
                    IsSuccess = true
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to analyze risk for document {DocumentId}",
                    request.DocumentId);

                await _auditService.LogAsync(new AuditLog
                {
                    DocumentId = request.DocumentId,
                    Operation = "RiskAnalysis",
                    InputSummary = $"Query: {request.Query}",
                    OutputSummary = "Analysis failed",
                    LatencyMs = stopwatch.Elapsed.TotalMilliseconds,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });

                return StatusCode(500,
                    new { Error = "Risk analysis failed",
                        Details = ex.Message });
            }
        }
    }
}

