namespace LoanLogic.Models
{
    public class LoanApplication
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal RequestedAmount { get; set; }
        public int TermMonths { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ReviewedAt { get; set; }
        public string? IdempotentKey { get; set; } = null;
    }
}
