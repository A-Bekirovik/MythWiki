using System;
using MythWikiBusiness.Models;

namespace MythWikiBusiness.DTO
{
	public class UserDTO
	{
        public int UserID { get; set; }
        public string Name { get; set; }
	}
}

namespace MythWikiBusiness.DTO
{
    public class UserDetailDTO
    {
        public int UserID { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public bool Admin { get; private set; }
    }
}


