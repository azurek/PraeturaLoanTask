using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
