using LoanLogic.Enums;
using System.Text.Json.Serialization;

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
