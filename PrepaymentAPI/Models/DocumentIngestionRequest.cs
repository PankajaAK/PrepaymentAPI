namespace PrepaymentAPI.Models
{
    public class DocumentIngestionRequest
    {
        /// <summary>
        /// Unique identifier for the loan document
        /// </summary>
        public string DocumentId { get; set; } = string.Empty;

        /// <summary>
        /// Base64 encoded document content or raw text
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Type of document - LoanAgreement, KYC, PrepaymentSchedule
        /// </summary>
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// Name of the borrower
        /// </summary>
        public string BorrowerName { get; set; } = string.Empty;

        /// <summary>
        /// Loan amount in INR
        /// </summary>
        public decimal LoanAmount { get; set; }

        /// <summary>
        /// Loan tenure in months
        /// </summary>
        public int TenureMonths { get; set; }

        /// <summary>
        /// Interest rate percentage
        /// </summary>
        public decimal InterestRate { get; set; }
    }
}

