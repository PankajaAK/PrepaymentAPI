namespace PrepaymentAPI.Models
{
    public class RiskAnalysisRequest
    {
        /// <summary>
        /// Document ID to analyze — links to ingested document
        /// </summary>
        public string DocumentId { get; set; } = string.Empty;

        /// <summary>
        /// Specific question or analysis query
        /// </summary>
        public string Query { get; set; } = string.Empty;
    }
}

