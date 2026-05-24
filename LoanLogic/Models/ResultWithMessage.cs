using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic.Models
{
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
