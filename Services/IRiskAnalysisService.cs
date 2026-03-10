using PrepaymentAPI.Models;

namespace PrepaymentAPI.Services
{
    public interface IRiskAnalysisService
    {
        Task<RiskAssessmentResponse> AnalyzeAsync(RiskAnalysisRequest request);
    }
}
