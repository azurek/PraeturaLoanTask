using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic.Models
{
    public class UnitResultWithMessage
    {
        public List<string> Messages { get; set; } = new List<string>();
        public bool IsValid => Messages == null || Messages.Count == 0;
    }
}
