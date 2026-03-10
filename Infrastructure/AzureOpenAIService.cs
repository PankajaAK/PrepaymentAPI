using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using PrepaymentAPI.Models;
using PrepaymentAPI.Services;

namespace PrepaymentAPI.Infrastructure
{
    public class AzureOpenAIService : IRiskAnalysisService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzureOpenAIService> _logger;

        public AzureOpenAIService(
            IConfiguration configuration,
            ILogger<AzureOpenAIService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<RiskAssessmentResponse> AnalyzeAsync(
            RiskAnalysisRequest request)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // Get configuration
                var endpoint = _configuration[
                    "AzureOpenAI:Endpoint"] ?? 
                    throw new InvalidOperationException(
                        "Azure OpenAI endpoint not configured");

                var apiKey = _configuration[
                    "AzureOpenAI:ApiKey"] ?? 
                    throw new InvalidOperationException(
                        "Azure OpenAI API key not configured");

                var deploymentName = _configuration[
                    "AzureOpenAI:DeploymentName"] ?? "gpt-4o";

                // Create client
                var client = new AzureOpenAIClient(
                    new Uri(endpoint),
                    new AzureKeyCredential(apiKey));

                var chatClient = client.GetChatClient(deploymentName);

                // Build financial system prompt
                var systemPrompt = """
                    You are a senior financial risk analyst 
                    specializing in mortgage prepayment risk assessment.

                    When analyzing loan documents or portfolio data:
                    1. Identify key prepayment risk indicators such as
                       loan-to-value ratio, interest rate differential,
                       borrower credit profile, and remaining tenure
                    2. Flag risk signals with severity HIGH, MEDIUM, or LOW
                    3. Provide a concise risk summary in plain language
                    4. Always cite which data points influenced your assessment
                    5. Never assume beyond provided data
                    6. End with a confidence score between 0 and 1

                    Respond ONLY in this exact JSON format:
                    {
                        "riskSummary": "your summary here",
                        "riskLevel": "HIGH or MEDIUM or LOW",
                        "riskFactors": ["factor1", "factor2", "factor3"],
                        "confidenceScore": 0.85
                    }
                    """;

                // Build user message with loan details
                var userMessage = $"""
                    Please analyze prepayment risk for this loan:
                    Document ID: {request.DocumentId}
                    Query: {request.Query}
                    """;

                // Call Azure OpenAI
                var response = await chatClient.CompleteChatAsync(
                    new ChatMessage[]
                    {
                        new SystemChatMessage(systemPrompt),
                        new UserChatMessage(userMessage)
                    });

                var content = response.Value.Content[0].Text;

                // Parse JSON response
                var jsonResponse = System.Text.Json.JsonSerializer
                    .Deserialize<AzureOpenAIRiskResponse>(
                        content,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                _logger.LogInformation(
                    "Azure OpenAI risk analysis completed in {Ms}ms",
                    stopwatch.Elapsed.TotalMilliseconds);

                return new RiskAssessmentResponse
                {
                    DocumentId = request.DocumentId,
                    RiskSummary = jsonResponse?.RiskSummary ?? 
                        "Unable to generate summary",
                    RiskLevel = jsonResponse?.RiskLevel ?? "UNKNOWN",
                    RiskFactors = jsonResponse?.RiskFactors ?? 
                        new List<string>(),
                    ConfidenceScore = jsonResponse?.ConfidenceScore ?? 0,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Azure OpenAI analysis failed for document {DocumentId}",
                    request.DocumentId);
                throw;
            }
        }
    }

    // Internal class to deserialize Azure OpenAI JSON response
    internal class AzureOpenAIRiskResponse
    {
        public string RiskSummary { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public List<string> RiskFactors { get; set; } = new();
        public double ConfidenceScore { get; set; }
    }
}
