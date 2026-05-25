namespace LoanLogic.Models
{
    /// <summary>
    /// Part or railway approach where result of a method is coupled with any error messages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultWithMessage<T>: UnitResultWithMessage
    {
        public T Result { get; set; }        

        public ResultWithMessage()
        {          
            if (typeof(T) != typeof(string))
            {
                Result = typeof(T).IsValueType ? default : Activator.CreateInstance<T>();
            }        
        }

    }
}
