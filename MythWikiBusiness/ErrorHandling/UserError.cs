using System;
namespace MythWikiBusiness.ErrorHandling
{
	public class UserError : Exception
	{
        public UserError(string message,Exception exception) : base(message,exception)
        {
        }

        public UserError(string message) : base(message)
        {
        }
    }
}

