using System;
using MythWikiBusiness.Models;
namespace MythWikiBusiness.ErrorHandling
{
	public class ServiceResponse
	{
		public bool Succes { get; set; }
		public string ErrorMessage { get; set; }
		public Subject Data { get; set; } // Need this so i can transfer specific data to a view that needs specific data
	}
}

