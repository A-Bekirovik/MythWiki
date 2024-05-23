using System;
namespace MythWikiBusiness.ErrorHandling
{
	public class SubjectError : Exception
	{
        public string Message { get; set; }
        public Exception exception { get; set; }

        public SubjectError(string message,Exception exception)
        {
            this.Message = message;
            this.exception = exception;
        }
    }
}

