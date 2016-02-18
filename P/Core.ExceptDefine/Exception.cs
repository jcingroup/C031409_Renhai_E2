using System;

/// <summary>
/// Exception 的摘要描述，系統錯誤及自訂錯誤。
/// </summary>

   public class ExceptionRoll : Exception
    {
        public ExceptionRoll()
            : base()
        {
        }

        public ExceptionRoll(String message)
            : base(message)
        {
        }

        public ExceptionRoll(String message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }

   public class LogicError : Exception
   {
       public LogicError()
           : base()
       {
       }

       public LogicError(String message)
           : base(message)
       {
       }

       public LogicError(String message, Exception innerException)
           : base(message, innerException)
       {
           
       }
   }