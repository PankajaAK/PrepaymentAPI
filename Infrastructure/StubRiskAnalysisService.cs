using PrepaymentAPI.Models;
using PrepaymentAPI.Services;

namespace PrepaymentAPI.Infrastructure
{
    public class StubRiskAnalysisService : IRiskAnalysisService
    {
        private readonly ILogger<StubRiskAnalysisService> _logger;

        public StubRiskAnalysisService(
            ILogger<StubRiskAnalysisService> logger)
        {
            _logger = logger;
        }

        public Task<RiskAssessmentResponse> AnalyzeAsync(
            RiskAnalysisRequest request)
        {
            _logger.LogInformation(
                "Analyzing risk for document {DocumentId}",
                request.DocumentId);

            // Stub implementation — real Azure OpenAI integration
            // comes in Week 2
            var response = new RiskAssessmentResponse
            {
                DocumentId = request.DocumentId,
                RiskSummary = "Stub: This document shows moderate " +
                              "prepayment risk based on loan parameters. " +
                              "Real Azure OpenAI analysis coming in Week 2.",
                RiskLevel = "MEDIUM",
                RiskFactors = new List<string>
                {
                    "Stub: High loan-to-value ratio detected",
                    "Stub: Interest rate differential above threshold",
                    "Stub: Remaining tenure under 24 months"
                },
                ConfidenceScore = 0.75
            };

            return Task.FromResult(response);
        }
    }
}