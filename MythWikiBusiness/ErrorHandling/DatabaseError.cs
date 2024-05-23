using System;
namespace MythWikiBusiness.ErrorHandling
{
	public class DatabaseError : Exception
	{
		public DatabaseError(string message, Exception exception) { }
	}
}
