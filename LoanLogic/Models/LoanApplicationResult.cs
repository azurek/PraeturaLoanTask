using LoanLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LoanLogic.Models
{
    public  class LoanApplicationResult
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LoanApplicationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
