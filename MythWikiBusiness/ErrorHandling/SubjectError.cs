using System;
namespace MythWikiBusiness.ErrorHandling
{
	public class SubjectError : Exception
    { 
        public SubjectError(string message,Exception exception) : base(message,exception)
        {

        }
        public SubjectError(string message) : base(message)
        {

        }
    }
}