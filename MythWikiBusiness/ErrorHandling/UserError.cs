using System;
namespace MythWikiBusiness.ErrorHandling
{
	public class UserError : Exception
	{
        public string Message { get; set; }
        public Exception exception { get; set; }

        public UserError(string message,Exception exception)
        {
            this.Message = message;
            this.exception = exception;
        }
    }
}

