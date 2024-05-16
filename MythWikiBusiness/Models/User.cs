using System;
using MythWikiBusiness.DTO;
namespace MythWikiBusiness.Models
{
	public class User
	{
        public int UserID { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public bool Admin { get; private set; }

        public User() 
	    { 
	    }

        public User(UserDTO userdto)
		{
            Name = userdto.Name;
            UserID = userdto.UserID;
		}
	}
}

