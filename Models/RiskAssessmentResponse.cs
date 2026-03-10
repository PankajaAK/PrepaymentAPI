namespace PrepaymentAPI.Models
{
    public class RiskAssessmentResponse
    {
        /// <summary>
        /// Document ID this assessment relates to
        /// </summary>
        public string DocumentId { get; set; } = string.Empty;

        /// <summary>
        /// AI generated prepayment risk summary
        /// </summary>
        public string RiskSummary { get; set; } = string.Empty;

        /// <summary>
        /// Risk level — HIGH, MEDIUM, LOW
        /// </summary>
        public string RiskLevel { get; set; } = string.Empty;

        /// <summary>
        /// Key risk factors identified
        /// </summary>
        public List<string> RiskFactors { get; set; } = new();

        /// <summary>
        /// AI confidence score 0 to 1
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Timestamp of assessment
        /// </summary>
        public DateTime AssessedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Audit trail ID for this assessment
        /// </summary>
        public string AuditId { get; set; } = Guid.NewGuid().ToString();
    }
}

