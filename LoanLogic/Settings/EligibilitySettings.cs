namespace LoanLogic.Settings
{
    public  class EligibilitySettings
    {
        public decimal MinimumMonthlyIncome { get; set; }
        public decimal MaximumRequestedAmountToMonthIncomeRatio { get; set; }
        public int MinimumTermMonths { get; set; }
        public int MaximumTermMonths { get; set; }
    }
}
