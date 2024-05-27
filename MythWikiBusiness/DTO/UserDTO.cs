using System;
using MythWikiBusiness.Models;

namespace MythWikiBusiness.DTO
{
	public class UserDTO
	{
        public int UserID { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
