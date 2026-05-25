namespace LoanLogic.Models
{
    /// <summary>
    /// Basic result class to be used when method doesnt return any object but we want to see if any errors occured
    /// </summary>
    public class UnitResultWithMessage
    {
        public List<string> Messages { get; set; } = new List<string>();
        public bool IsValid => Messages == null || Messages.Count == 0;
    }
}
