namespace LoanLogic.Models
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<string, string> _errorMessages = new()
        {
            { ErrorCode.E001, "Name is required" },
            { ErrorCode.E002, "Email is required" },
            { ErrorCode.E003, "Email is not valid" },
            { ErrorCode.E004, "Name and surname is required, found only one" },
            { ErrorCode.E005, "Monthly income cannot be null" },
            { ErrorCode.E006, "Monthly income has to be positive" },
            { ErrorCode.E007, "Requested amount cannot be null" },
            { ErrorCode.E008, "Requested amount has to be positive" },
            { ErrorCode.E009, "Term months cannot be null" },
            { ErrorCode.E010, "Term months has to be positive" },
            { ErrorCode.E011, "Failed to save locan appliaction due to: '{0}'" },
            { ErrorCode.E012, "Unable to find laon application with id '{0}'" },
            { ErrorCode.E013, "Failed to save decision log entries due to: '{0}'" },
            { ErrorCode.E014, "Failed to update loan application due to: '{0}'" },
            { ErrorCode.E015, "Request cancelled"   }

        };
        
        public static string GetMessage(string errorCode)
        {
            return _errorMessages.TryGetValue(errorCode, out var message) ? message : "Unknown error";
        }
    }
}
