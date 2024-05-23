using System;
namespace MythWikiBusiness.ErrorHandling
{
	public class DatabaseError : Exception
	{
		public string Message { get; set; }
		public Exception exception { get; set; }

		public DatabaseError(string message, Exception exception) 
		{
			this.Message = message;
			this.exception = exception; 
		}
	}
}
